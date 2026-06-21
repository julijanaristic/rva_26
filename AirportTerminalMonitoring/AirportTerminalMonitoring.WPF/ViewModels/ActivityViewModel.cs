using AirportTerminalMonitoring.Domain.Enums;
using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Services.Interfaces;
using AirportTerminalMonitoring.Services.Simulation;
using AirportTerminalMonitoring.WPF.Commands;
using AirportTerminalMonitoring.WPF.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Input;
using System.Windows.Threading;

namespace AirportTerminalMonitoring.WPF.ViewModels
{
    public class ActivityViewModel : BaseViewModel
    {
        private readonly IRepository<TerminalActivity> _repository;
        private readonly ILoggerService _logger;
        private readonly IDataStorageService _storageService;
        private readonly IRepository<AirportTerminal> _terminalRepository;

        private readonly DispatcherTimer _timer;
        private readonly TerminalStateSimulationService _simulationService;

        private ObservableCollection<TerminalActivity> _activities;

        public ObservableCollection<TerminalActivity> Activities
        {
            get => _activities;
            set
            {
                _activities = value;
                OnPropertyChanged();
            }
        }

        private TerminalActivity _selectedActivity;
        public TerminalActivity SelectedActivity
        {
            get => _selectedActivity;
            set
            {
                _selectedActivity = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SearchCommand { get; }

        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Axis> _xAxes;
        public IEnumerable<Axis> XAxes
        {
            get => _xAxes;
            set
            {
                _xAxes = value;
                OnPropertyChanged();
            }
        }

        public string[] Labels => new[]
        {
            "Operational",
            "Congested",
            "Delayed",
            "Closed"
        };

        public ActivityViewModel(IRepository<TerminalActivity> repository, ILoggerService logger, IDataStorageService storageService, IRepository<AirportTerminal> terminalRepository)
        {
            _repository = repository;
            _logger = logger;
            _storageService = storageService;
            _terminalRepository = terminalRepository;
            Activities = new ObservableCollection<TerminalActivity>(_repository.GetAll());
            AddCommand = new RelayCommand(_ => AddActivity());
            DeleteCommand = new RelayCommand(_ => DeleteActivity());
            EditCommand = new RelayCommand(_ => EditActivity());
            SearchCommand = new RelayCommand(_ => SearchActivity());
            _simulationService = new TerminalStateSimulationService();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += Timer_Tick;
            _timer.Start(); 
        }

        private void AddActivity()
        {
            ActivityFormViewModel form = new ActivityFormViewModel(_terminalRepository.GetAll());

            form.SaveRequested += activity =>
            {
                _repository.Add(activity);
                Activities.Add(activity);
                _storageService.SaveActivities(Activities);
                _logger.Log($"Added activity {activity.Id}");
            };
            ActivityFormWindow window = new ActivityFormWindow(form);
            window.ShowDialog();
        }

        private void DeleteActivity()
        {
            if (SelectedActivity == null)
                return;

            _repository.Delete(SelectedActivity.Id);
            Activities.Remove(SelectedActivity);
            _storageService.SaveActivities(Activities);
            _logger.Log($"Deleted activity {SelectedActivity.Id}");
        }

        private void EditActivity()
        {
            if (SelectedActivity == null)
                return;

            ActivityFormViewModel form = new ActivityFormViewModel(SelectedActivity, _terminalRepository.GetAll());

            form.SaveRequested += activity =>
            {
                _repository.Update(activity);
                int index = Activities.IndexOf(SelectedActivity);
                Activities[index] = activity;
                SelectedActivity = activity;
                _storageService.SaveActivities(Activities);
                _logger.Log($"Updated activity {activity.Id}");
            };
            ActivityFormWindow window = new ActivityFormWindow(form);
            window.ShowDialog();
        }

        private void SearchActivity()
        {
            var activites = _repository.GetAll();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Activities = new ObservableCollection<TerminalActivity>(activites);
                return;
            }

            string search = SearchText.ToLower();

            var filtered = activites.Where(a => 
                    a.PassengerCount.ToString().Contains(search)
                    || a.AverageWaitTime.ToString().Contains(search)
                    || a.NumberOfDelays.ToString().Contains(search)
                    || a.State.ToString().ToLower().Contains(search)
                    || a.CollectionTime.ToString().ToLower().Contains(search)
            );

            Activities = new ObservableCollection<TerminalActivity>(filtered);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var a in Activities)
            {
                a.State = _simulationService.GetNextState(a.State);
            }
            OnPropertyChanged(nameof(Activities));

            UpdateChart();
        }

        private void UpdateChart()
        {
            int operational = Activities.Count(a => a.State == TerminalState.Operational);
            int congested = Activities.Count(a => a.State == TerminalState.Congested);
            int delayed = Activities.Count(a => a.State == TerminalState.Delayed);
            int closed = Activities.Count(a => a.State == TerminalState.Closed);

            Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = new int[]
                    {
                        operational, congested, delayed, closed
                    }
                }
            };

            XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = Labels
                }
            };
        }
    }
}
