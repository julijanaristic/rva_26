using AirportTerminalMonitoring.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirportTerminalMonitoring.WPF.Views
{
    /// <summary>
    /// Interaction logic for ActivityFormWindow.xaml
    /// </summary>
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
