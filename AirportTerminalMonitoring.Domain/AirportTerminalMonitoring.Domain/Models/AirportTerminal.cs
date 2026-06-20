using System;

namespace AirportTerminalMonitoring.Domain.Models
{
    public class AirportTerminal
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string AirportName { get; set; }
        public int GateCount { get; set; }
        public bool DirectExit { get; set; }
    }
}
