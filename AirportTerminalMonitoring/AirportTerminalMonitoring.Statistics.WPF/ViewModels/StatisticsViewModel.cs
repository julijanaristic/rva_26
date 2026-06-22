using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Statistics.Services.Interfaces;
using AirportTerminalMonitoring.Statistics.Services.Services;
using AirportTerminalMonitoring.Statistics.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportTerminalMonitoring.Statistics.WPF.ViewModels
{
    public enum StatisticsMethod
    {
        PassengerAndDelays,
        MaxWaitTime,
        ClosedCount
    }
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly IWcfDataService _wcfService;
        private readonly FallbackDataService _fallbackService;
        private readonly IStatisticsService _statisticsService;
        private readonly CsvExportService _csvExport;

        private Dictionary<string, List<TerminalActivity>> _groupedData
           = new Dictionary<string, List<TerminalActivity>>();

        private List<AirportTerminal> _terminals = new List<AirportTerminal>();

        // Datumi za filtriranje
        private DateTime _fromDate = DateTime.Today.AddDays(-7);
        public DateTime FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); }
        }
        private DateTime _toDate = DateTime.Today;
        public DateTime ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); }
        }

        // Prikaz grupisanih podataka
        private ObservableCollection<string> _displayLines = new ObservableCollection<string>();
        public ObservableCollection<string> DisplayLines
        {
            get => _displayLines;
            set { _displayLines = value; OnPropertyChanged(); }
        }

        // Izabrana statisticka metoda
        private StatisticsMethod _selectedMethod = StatisticsMethod.PassengerAndDelays;
        public StatisticsMethod SelectedMethod
        {
            get => _selectedMethod;
            set { _selectedMethod = value; OnPropertyChanged(); }
        }

        public Array Methods => Enum.GetValues(typeof(StatisticsMethod));

        // Rezultat statistike
        private string _statisticsResult;
        public string StatisticsResult
        {
            get => _statisticsResult;
            set { _statisticsResult = value; OnPropertyChanged(); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand ComputeCommand { get; }
        public ICommand ExportCsvCommand { get; }

        public StatisticsViewModel(IWcfDataService wcfService, IStatisticsService statisticsService)
        {
            _wcfService = wcfService;
            _fallbackService = new FallbackDataService();
            _statisticsService = statisticsService;
            _csvExport = new CsvExportService();

            LoadDataCommand = new RelayCommand(LoadData);
            ComputeCommand = new RelayCommand(Compute);
            ExportCsvCommand = new RelayCommand(ExportCsv);
        }

        private void LoadData()
        {
            List<TerminalActivity> activities;

            try
            {
                // Pokušaj da povučeš podatke preko WCF-a (TCP)
                activities = _wcfService.GetActivities();
                _terminals = _wcfService.GetTerminals();
                StatusMessage = $"Loaded {activities.Count} activities from Component 1 via WCF.";
            }
            catch
            {
                // FALLBACK: Ako WCF server nije upaljen (pukne veza), aktivira se tvoj FallbackDataService
                // Ovde dodajemo i povlačenje 3 podrazumevana terminala!
                activities = _fallbackService.GetDefaultActivities();
                _terminals = _fallbackService.GetDefaultTerminals();
                StatusMessage = "WCF connection unavailable. Using fallback data (3 instances).";
            }

            // --- OSTATAK METODE OSTAJE POTPUNO ISTI KAO ŠTO VEĆ IMAŠ ---

            // Filtriraj po periodu i grupisaj u Dictionary<"Datum1-Datum2", List<TerminalActivity>>
            string key = $"{FromDate:yyyy-MM-dd}, {ToDate:yyyy-MM-dd}";

            var filtered = activities
                .Where(a => a.CollectionTime >= FromDate && a.CollectionTime <= ToDate)
                .ToList();

            _groupedData = new Dictionary<string, List<TerminalActivity>>
    {
        { key, filtered }
    };

            UpdateDisplayLines();
        }
        private void UpdateDisplayLines()
        {
            DisplayLines.Clear();

            foreach (var entry in _groupedData)
            {
                // Grupisemo aktivnosti po terminalu
                var byTerminal = entry.Value.GroupBy(a => a.TerminalId);

                foreach (var group in byTerminal)
                {
                    string terminalName = _terminals
                        .FirstOrDefault(t => t.Id == group.Key)?.Code
                        ?? group.Key.ToString().Substring(0, 8);

                    // Format: [BrojPutnika, VremeCekanjamin, BrojKasnjenja]
                    string activities = string.Join(", ",
                        group.Select(a => $"[{a.PassengerCount}, {a.AverageWaitTime}min, {a.NumberOfDelays}]"));

                    DisplayLines.Add($"({entry.Key}): {terminalName} -> {activities}");
                }
            }
        }

        private void Compute()
        {
            if (_groupedData.Count == 0)
            {
                StatisticsResult = "No data loaded. Please load data first.";
                return;
            }

            StringBuilder sb = new StringBuilder();

            switch (SelectedMethod)
            {
                case StatisticsMethod.PassengerAndDelays:
                    var stats1 = _statisticsService.GetPassengerAndDelayStats(_groupedData);
                    sb.AppendLine("Max Passengers & Avg Delays per Terminal:");
                    foreach (var s in stats1)
                        sb.AppendLine($"Terminal {s.Key}: Max={s.Value.MaxPassengers} passengers, Avg delays={s.Value.AvgDelays:F1}");
                    break;

                case StatisticsMethod.MaxWaitTime:
                    var stats2 = _statisticsService.GetMaxWaitTimeStats(_groupedData);
                    sb.AppendLine("Max Average Wait Time per Terminal:");
                    foreach (var s in stats2)
                        sb.AppendLine($"Terminal {s.Key}: {s.Value.MaxWaitTime} min on {s.Value.Date:yyyy-MM-dd}");
                    break;

                case StatisticsMethod.ClosedCount:
                    var stats3 = _statisticsService.GetClosedCount(_groupedData);
                    sb.AppendLine("Times Closed per Terminal:");
                    foreach (var s in stats3)
                        sb.AppendLine($"Terminal {s.Key}: {s.Value} times closed");
                    break;
            }

            StatisticsResult = sb.ToString();
        }

        private void ExportCsv()
        {
            if (string.IsNullOrWhiteSpace(StatisticsResult))
            {
                StatisticsResult = "Nothing to export. Run statistics first.";
                return;
            }

            // Otvaramo dijalog direktno iz WPF ViewModela
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "statistics.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                // Prosleđujemo i tekst i putanju fajla koju je korisnik izabrao
                _csvExport.Export(StatisticsResult, dialog.FileName);
                StatusMessage = "CSV successfully exported!";
            }
        }
    }
}
