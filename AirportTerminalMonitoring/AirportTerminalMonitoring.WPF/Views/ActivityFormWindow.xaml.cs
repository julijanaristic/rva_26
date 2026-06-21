using AirportTerminalMonitoring.WPF.ViewModels;
using System.Windows;

namespace AirportTerminalMonitoring.WPF.Views
{
    public partial class ActivityFormWindow : Window
    {
        public ActivityFormWindow(ActivityFormViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.SaveRequested += activity =>
            {
                DialogResult = true;
                Close();
            };

            viewModel.CancelRequested += () =>
            {
                DialogResult = false;
                Close();
            };
        }
    }
}
