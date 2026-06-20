using AirportTerminalMonitoring.Domain.Enums;
using System;

namespace AirportTerminalMonitoring.Domain.Models
{
    public class TerminalActivity
    {
        public Guid Id { get; set; }
        public Guid TerminalId { get; set; }
        public DateTime CollectionTime { get; set; }
        public int PassengerCount { get; set; }
        public double AverageWaitTime { get; set; }
        public int NumberOfDelays { get; set; }
        public TerminalState State { get; set; }
    }
}
