using AirportTerminalMonitoring.Services.Logging;
using AirportTerminalMonitoring.Services.Persistence;
using AirportTerminalMonitoring.Services.Repositories;
using AirportTerminalMonitoring.WPF.ViewModels;
using AirportTerminalMonitoring.WPF.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirportTerminalMonitoring.WPF
{
    public partial class MainWindow : Window
    {
        private readonly TerminalRepository _terminalRepository;
        private readonly TerminalActivityRepository _activityRepository;
        private readonly FileLogger _logger;
        private readonly XmlStorageService _storage;

        public MainWindow()
        {
            InitializeComponent();

            _terminalRepository = new TerminalRepository();
            _activityRepository = new TerminalActivityRepository();
            _logger = new FileLogger();
            _storage = new XmlStorageService();
            LoadData();
        }

        private void Terminals_Click(object sender, RoutedEventArgs e)
        {
            TerminalViewModel vm = new TerminalViewModel(_terminalRepository, _logger, _storage);
            TerminalWindow window = new TerminalWindow(vm);
            window.ShowDialog();
        }

        private void Activities_Click(object sender, RoutedEventArgs e)
        {
            ActivityViewModel vm = new ActivityViewModel(_activityRepository, _logger, _storage, _terminalRepository);
            ActivityWindow window = new ActivityWindow(vm);
            window.ShowDialog();
        }

        private void LoadData()
        {
            var terminals = _storage.LoadTerminals();

            foreach (var t in terminals)
                _terminalRepository.Add(t);

            var activities = _storage.LoadActivities();

            foreach (var a in activities)
                _activityRepository.Add(a);
        }
    }
}