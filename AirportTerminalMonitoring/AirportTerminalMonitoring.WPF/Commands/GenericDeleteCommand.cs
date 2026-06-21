using AirportTerminalMonitoring.Services.Interfaces;
using System.Collections.ObjectModel;

namespace AirportTerminalMonitoring.WPF.Commands
{
    public class GenericDeleteCommand<T> : IUndoRedoCommand where T : class
    {
        private readonly ObservableCollection<T> _collection;
        private readonly IRepository<T> _repository;
        private readonly T _entity;
        private int _removedIndex;

        public GenericDeleteCommand(ObservableCollection<T> collection, IRepository<T> repository, T entity)
        {
            _collection = collection;
            _repository = repository;
            _entity = entity;
        }

        public void Execute()
        {
            _removedIndex = _collection.IndexOf(_entity);
            var idProperty = _entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                Guid id = (Guid)idProperty.GetValue(_entity);
                _repository.Delete(id);
                _collection.Remove(_entity);
            }
        }

        public void Undo()
        {
            _repository.Add(_entity);
            _collection.Insert(_removedIndex, _entity);
        }
    }
}
