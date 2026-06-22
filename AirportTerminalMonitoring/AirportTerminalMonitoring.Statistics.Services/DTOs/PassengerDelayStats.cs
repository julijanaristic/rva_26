using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.DTOs
{
    public class PassengerDelayStats
    {
        public int MaxPassengers { get; set; }
        public double AvgDelays { get; set; }

    }
}
