using System;
using System.ComponentModel;

namespace AirportTerminalMonitoring.Domain.Models
{
    public class AirportTerminal : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        private string _code;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
        private string _airportName;
        public string AirportName
        {
            get => _airportName;
            set
            {
                _airportName = value;
                OnPropertyChanged(nameof(AirportName));
            }
        }
        private int _gateCount;
        public int GateCount
        {
            get => _gateCount;
            set
            {
                _gateCount = value;
                OnPropertyChanged(nameof(GateCount));
            }
        }
        private bool _directExit;
        public bool DirectExit
        {
            get => _directExit;
            set
            {
                _directExit = value;
                OnPropertyChanged(nameof(DirectExit));
            }
        }
    }
}
