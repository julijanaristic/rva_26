using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.WPF.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace AirportTerminalMonitoring.WPF.ViewModels
{
    public class TerminalFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        private string _airportName;
        public string AirportName
        {
            get => _airportName;
            set
            {
                _airportName = value;
                OnPropertyChanged();
            }
        }

        private int _gateCount;
        public int GateCount
        {
            get => _gateCount;
            set
            {
                _gateCount = value;
                OnPropertyChanged();
            }
        }

        public bool _directExit;
        public bool DirectExit
        {
            get => _directExit;
            set
            {
                _directExit = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get;  }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case nameof(Code):
                        if (string.IsNullOrWhiteSpace(Code))
                            return "Code is required";
                        if (Code.Length < 2)
                            return "Code must contain at least 2 characters.";
                        break;
                    case nameof(AirportName):
                        if (string.IsNullOrWhiteSpace(AirportName))
                            return "Airport name is required.";
                        break;
                    case nameof(GateCount):
                        if (GateCount <= 0)
                            return "Gate count must be greater than 0.";
                        break;
                }
                return null;
            }
        }

        public event Action<AirportTerminal> SaveRequested;
        public event Action CancelRequested;

        public TerminalFormViewModel()
        {
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public TerminalFormViewModel(AirportTerminal terminal) : this()
        {
            Id = terminal.Id;
            Code = terminal.Code;
            AirportName = terminal.AirportName;
            GateCount = terminal.GateCount;
            DirectExit = terminal.DirectExit;
        }

        private void Save()
        {
            if (!IsValid())
                return;

            AirportTerminal terminal = new AirportTerminal
            {
                Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
                Code = Code,
                AirportName = AirportName,
                GateCount = GateCount,
                DirectExit = DirectExit
            };
            SaveRequested?.Invoke(terminal);
        }

        private void Cancel()
        {
            CancelRequested?.Invoke();
        }

        private bool IsValid()
        {
            return this[nameof(Code)] == null && this[nameof(AirportName)] == null && this[nameof(GateCount)] == null;
        }
    }
}
