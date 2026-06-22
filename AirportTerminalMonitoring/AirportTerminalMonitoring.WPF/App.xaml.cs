using AirportTerminalMonitoring.WCF;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace AirportTerminalMonitoring.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = new ServiceHost(typeof(TerminalService));

            _host.AddServiceEndpoint(
                typeof(ITerminalService),
                new NetTcpBinding(),
                "net.tcp://localhost:8080/TerminalService");

            _host.Open();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_host != null)
                _host.Close();

            base.OnExit(e);
        }
    }
}
