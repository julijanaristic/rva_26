using AirportTerminalMonitoring.WPF.ViewModels;
using System.Windows;

namespace AirportTerminalMonitoring.WPF.Views
{
    public partial class ActivityWindow : Window
    {
        public ActivityWindow(ActivityViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
