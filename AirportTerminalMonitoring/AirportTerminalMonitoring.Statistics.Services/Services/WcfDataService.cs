using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Statistics.Services.Interfaces;
using AirportTerminalMonitoring.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Services
{
    public class WcfDataService : IWcfDataService
    {
        private readonly string _address = "net.tcp://localhost:8080/TerminalService";
        public List<TerminalActivity> GetActivities()
        {
            ChannelFactory<ITerminalService> factory = new ChannelFactory<ITerminalService>(
                            new NetTcpBinding(),
                            new EndpointAddress(_address));

            ITerminalService client = factory.CreateChannel();
            return client.GetAllActivities();
        }

        public List<AirportTerminal> GetTerminals()
        {
            ChannelFactory<ITerminalService> factory = new ChannelFactory<ITerminalService>(
                            new NetTcpBinding(),
                            new EndpointAddress(_address));

            ITerminalService client = factory.CreateChannel();
            return client.GetAllTerminals();
        }

    }
}
