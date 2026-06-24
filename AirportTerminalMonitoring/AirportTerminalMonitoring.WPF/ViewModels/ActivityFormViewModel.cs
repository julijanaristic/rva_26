using AirportTerminalMonitoring.Domain.Enums;
using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AirportTerminalMonitoring.WPF.ViewModels
{
    public class ActivityFormViewModel : BaseViewModel, IDataErrorInfo
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

        private Guid _terminalId;
        public Guid TerminalId
        {
            get => _terminalId;
            set
            {
                _terminalId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _collectionTime;
        public DateTime CollectionTime
        {
            get => _collectionTime;
            set
            {
                _collectionTime = value;
                OnPropertyChanged();
            }
        }

        private int _passengerCount;
        public int PassengerCount
        {
            get => _passengerCount;
            set
            {
                _passengerCount = value;
                OnPropertyChanged();
            }
        }

        private double _averageWaitTime;
        public double AverageWaitTime
        {
            get => _averageWaitTime;
            set
            {
                _averageWaitTime = value;
                OnPropertyChanged();
            }
        }

        private int _numberOfDelays;
        public int NumberOfDelays
        {
            get => _numberOfDelays;
            set
            {
                _numberOfDelays = value;
                OnPropertyChanged();
            }
        }

        private TerminalState _state;
        public TerminalState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public Array States => Enum.GetValues(typeof(TerminalState));

        private AirportTerminal _selectedTerminal;
        public AirportTerminal SelectedTerminal
        {
            get => _selectedTerminal;
            set
            {
                _selectedTerminal = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AirportTerminal> Terminals
        {
            get;
            set;
        } = new ObservableCollection<AirportTerminal>();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(PassengerCount):
                        if (PassengerCount < 0)
                            return "Passenger count cannot be negative.";
                        break;

                    case nameof(AverageWaitTime):
                        if (AverageWaitTime < 0)
                            return "Average wait time cannot be negative.";
                        break;

                    case nameof(NumberOfDelays):
                        if (NumberOfDelays < 0)
                            return "Number of delays cannot be negative.";
                        break;

                    case nameof(SelectedTerminal):
                        if (SelectedTerminal == null)
                            return "You must choose a terminal";
                        break;

                }
                return null;
            }
        }

        public event Action<TerminalActivity> SaveRequested;
        public event Action CancelRequested;

        public ActivityFormViewModel(IEnumerable<AirportTerminal> terminals)
        {
            Terminals = new ObservableCollection<AirportTerminal>(terminals);
            CollectionTime = DateTime.Today;
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }


        public ActivityFormViewModel(TerminalActivity activity, IEnumerable<AirportTerminal> terminals) : this(terminals)
        {
            Id = activity.Id;
            TerminalId = activity.TerminalId;
            CollectionTime = activity.CollectionTime;
            PassengerCount = activity.PassengerCount;
            AverageWaitTime = activity.AverageWaitTime;
            NumberOfDelays = activity.NumberOfDelays;
            State = activity.State;

            SelectedTerminal = Terminals.FirstOrDefault(t => t.Id == activity.TerminalId);
        }

        private void Save()
        {
            if (!IsValid(out string errors))
            {
                MessageBox.Show(
                    errors,
                    "Validation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            TerminalActivity activity = new TerminalActivity
            {
                Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
                TerminalId = SelectedTerminal.Id,
                CollectionTime = CollectionTime,
                PassengerCount = PassengerCount,
                AverageWaitTime = AverageWaitTime,
                NumberOfDelays = NumberOfDelays,
                State = State
            };
            SaveRequested?.Invoke(activity);
        }

        private void Cancel()
        {
            CancelRequested?.Invoke();
        }

        private bool IsValid(out string errors)
        {
            List<string> validationErrors = new List<string>();

            foreach (string property in new[]
            {
                nameof(PassengerCount),
                nameof(AverageWaitTime),
                nameof(NumberOfDelays),
                nameof(SelectedTerminal)
            })
            {
                string error = this[property];

                if (!string.IsNullOrEmpty(error))
                {
                    validationErrors.Add(error);
                }
            }

            errors = string.Join(Environment.NewLine, validationErrors);

            return validationErrors.Count == 0;
        }

       
    }
}
