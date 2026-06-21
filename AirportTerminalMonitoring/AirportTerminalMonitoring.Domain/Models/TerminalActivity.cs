using AirportTerminalMonitoring.Domain.Enums;
using System;
using System.ComponentModel;

namespace AirportTerminalMonitoring.Domain.Models
{
    public class TerminalActivity : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public Guid TerminalId { get; set; }
        public DateTime CollectionTime { get; set; }
        public int PassengerCount { get; set; }
        public double AverageWaitTime { get; set; }
        public int NumberOfDelays { get; set; }

        private TerminalState _state;
        public TerminalState State
        {
            get => _state;
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
