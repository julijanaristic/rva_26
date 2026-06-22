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
    /// Interaction logic for TerminalFormWindow.xaml
    /// </summary>
    public partial class TerminalFormWindow : Window
    {
        public TerminalFormWindow(TerminalFormViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.SaveRequested += terminal =>
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
