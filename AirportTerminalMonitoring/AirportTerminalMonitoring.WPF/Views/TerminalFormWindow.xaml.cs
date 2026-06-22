using AirportTerminalMonitoring.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AirportTerminalMonitoring.WPF.Views
{
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
