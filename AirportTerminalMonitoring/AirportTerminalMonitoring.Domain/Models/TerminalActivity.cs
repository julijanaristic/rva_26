using AirportTerminalMonitoring.Domain.Enums;
using System;
using System.ComponentModel;

namespace AirportTerminalMonitoring.Domain.Models
{
    public class TerminalActivity : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        private Guid _terminalId;
        public Guid TerminalId
        {
            get => _terminalId;
            set
            {
                _terminalId = value;
                OnPropertyChanged(nameof(TerminalId));
            }
        }
        private DateTime _collectionTime;
        public DateTime CollectionTime
        {
            get => _collectionTime;
            set
            {
                _collectionTime = value;
                OnPropertyChanged(nameof(CollectionTime));
            }
        }
        private int _passengerCount;
        public int PassengerCount
        {
            get => _passengerCount;
            set
            {
                _passengerCount = value;
                OnPropertyChanged(nameof(PassengerCount));
            }
        }
        private double _averageWaitTime;
        public double AverageWaitTime
        {
            get => _averageWaitTime;
            set
            {
                _averageWaitTime = value;
                OnPropertyChanged(nameof(AverageWaitTime));
            }
        }
        private int _numberOfDelays;
        public int NumberOfDelays
        {
            get => _numberOfDelays;
            set
            {
                _numberOfDelays = value;
                OnPropertyChanged(nameof(NumberOfDelays));
            }
        }

        private TerminalState _state;
        public TerminalState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
