using AirportTerminalMonitoring.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.WPF.Commands
{
    public class GenericEditCommand<T> : IUndoRedoCommand where T : class
    {
        private readonly ObservableCollection<T> _collection;
        private readonly IRepository<T> _repository;
        private readonly T _oldState;
        private readonly T _newState;
        private readonly T _currentReference;

        public GenericEditCommand(ObservableCollection<T> collection, IRepository<T> repository, T oldState, T newState, T currentReference)
        {
            _collection = collection;
            _repository = repository;
            _oldState = oldState;
            _newState = newState;
            _currentReference = currentReference;
        }

        public void Execute()
        {
            ApplyState(_newState);
        }

        public void Undo()
        {
            ApplyState(_oldState);
        }

        private void ApplyState(T state)
        {
            _repository.Update(state);
            int index = _collection.IndexOf(_currentReference);
            if (index >= 0)
            {
                _collection[index] = state;
            }
        }
    }
}
