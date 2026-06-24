using AirportTerminalMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Interfaces
{
    public interface IFallbackDataService
    {
        List<TerminalActivity> GetDefaultActivities();
        List<AirportTerminal> GetDefaultTerminals();
    }
}
