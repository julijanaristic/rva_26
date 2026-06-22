using AirportTerminalMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Interfaces
{
    public interface IWcfDataService
    {
        List<TerminalActivity> GetActivities();
        List<AirportTerminal> GetTerminals();

    }
}
