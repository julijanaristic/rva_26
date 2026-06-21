using AirportTerminalMonitoring.WPF.ViewModels;
using System.Windows;

namespace AirportTerminalMonitoring.WPF.Views
{
    public partial class TerminalWindow : Window
    {
        public TerminalWindow(TerminalViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
