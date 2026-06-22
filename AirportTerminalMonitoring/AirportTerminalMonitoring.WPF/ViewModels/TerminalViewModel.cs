using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Services.Interfaces;
using AirportTerminalMonitoring.WCF;
using AirportTerminalMonitoring.WPF.Commands;
using AirportTerminalMonitoring.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportTerminalMonitoring.WPF.ViewModels
{
    public class TerminalViewModel : BaseViewModel
    {
        private readonly IRepository<AirportTerminal> _repository;
        private readonly ILoggerService _logger;
        private readonly IDataStorageService _storage;

        private ObservableCollection<AirportTerminal> _terminals;

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

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AirportTerminal> Terminals
        {
            get => _terminals;
            set
            {
                _terminals = value;
                OnPropertyChanged();
            }

        }

        private readonly Stack<IUndoRedoCommand> _undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> _redoStack = new Stack<IUndoRedoCommand>();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public TerminalViewModel(IRepository<AirportTerminal> repository, ILoggerService logger, IDataStorageService storage)
        {
            _repository = repository;
            _logger = logger;
            _storage = storage;
            Terminals = new ObservableCollection<AirportTerminal>(_repository.GetAll());
            TerminalService.Terminals = Terminals.ToList();
            AddCommand = new RelayCommand(_ => AddTerminal());
            DeleteCommand = new RelayCommand(_ => DeleteTerminal());
            EditCommand = new RelayCommand(_ => EditTerminal());
            SearchCommand = new RelayCommand(_ => SearchTerminals());
            UndoCommand = new RelayCommand(_ => Redo(), _ => _undoStack.Count > 0);
            RedoCommand = new RelayCommand(_ => Undo(), _ => _redoStack.Count > 0);
        }

        private void AddTerminal()
        {
            TerminalFormViewModel form = new TerminalFormViewModel();
            form.SaveRequested += terminal =>
            {
                var cmd = new GenericAddCommand<AirportTerminal>(Terminals, _repository, terminal);
                cmd.Execute();

                _undoStack.Push(cmd);
                _redoStack.Clear();

                _storage.SaveTerminals(Terminals);
                _logger.Log($"Added terminal {terminal.Code}");
                TerminalService.Terminals = Terminals.ToList();
            };
            TerminalFormWindow window = new TerminalFormWindow(form);
            window.ShowDialog();
        }

        private void DeleteTerminal()
        {
            if (SelectedTerminal == null)
                return;

            var cmd = new GenericDeleteCommand<AirportTerminal>(Terminals, _repository, SelectedTerminal);
            _logger.Log($"Deleted terminal {SelectedTerminal.Code}");

            cmd.Execute();
            _undoStack.Push(cmd);
            _redoStack.Clear();

            _storage.SaveTerminals(Terminals);
            TerminalService.Terminals = Terminals.ToList();
        }

        private void EditTerminal()
        {
            if (SelectedTerminal == null)
                return;

            var oldState = new AirportTerminal
            {
                Id = SelectedTerminal.Id,
                Code = SelectedTerminal.Code,
                AirportName = SelectedTerminal.AirportName,
                GateCount = SelectedTerminal.GateCount,
                DirectExit = SelectedTerminal.DirectExit
            };

            TerminalFormViewModel form = new TerminalFormViewModel(SelectedTerminal);

            form.SaveRequested += terminal =>
            {
                var cmd = new GenericEditCommand<AirportTerminal>(Terminals, _repository, oldState, terminal, SelectedTerminal);
                cmd.Execute();

                _undoStack.Push(cmd);
                _redoStack.Clear();

                SelectedTerminal = terminal;
                _storage.SaveTerminals(Terminals);
                _logger.Log($"Updated terminal {terminal.Code}");
                TerminalService.Terminals = Terminals.ToList();
            }; 

            TerminalFormWindow window = new TerminalFormWindow(form);
            window.ShowDialog();
        }

        private void SearchTerminals()
        {
            var terminals = _repository.GetAll();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Terminals = new ObservableCollection<AirportTerminal>(terminals);
                return;
            }

            string search = SearchText.ToLower();

            var filtered = terminals.Where(t =>
                    t.Code.ToLower().Contains(search)
                    || t.AirportName.ToLower().Contains(search)
                    || t.GateCount.ToString().ToLower().Contains(search)
                    || t.DirectExit.ToString().ToLower().Contains(search)
            );

            Terminals = new ObservableCollection<AirportTerminal>(filtered);
            TerminalService.Terminals = Terminals.ToList();
        }

        private void Undo()
        {
            if (_undoStack.Count == 0)
                return;
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            _storage.SaveTerminals(Terminals);
            _logger.Log("Undo executed.");
            TerminalService.Terminals = Terminals.ToList();
        }

        private void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            _storage.SaveTerminals(Terminals);
            _logger.Log("Redo executed.");
            TerminalService.Terminals = Terminals.ToList();
        }
    }
}
