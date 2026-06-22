using AirportTerminalMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.WCF
{
    public class TerminalService : ITerminalService
    {
        public static List<TerminalActivity> Activities { get; set; } = new List<TerminalActivity>();
        public static List<AirportTerminal> Terminals { get; set; } = new List<AirportTerminal>();
        public List<TerminalActivity> GetAllActivities()
        {
            return Activities;
        }

        public List<AirportTerminal> GetAllTerminals()
        {
            return Terminals;
        }
    }
}
