using AirportTerminalMonitoring.Domain.Models;
using System.Collections.Generic;

namespace AirportTerminalMonitoring.Services.Interfaces
{
    public interface IDataStorageService
    {
        void SaveTerminals(IEnumerable<AirportTerminal> terminals);
        List<AirportTerminal> LoadTerminals();
        void SaveActivities(IEnumerable<TerminalActivity> activities);
        List<TerminalActivity> LoadActivities();
    }
}
