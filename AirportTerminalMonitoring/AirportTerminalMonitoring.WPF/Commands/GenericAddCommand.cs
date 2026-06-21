using AirportTerminalMonitoring.Services.Interfaces;
using System.Collections.ObjectModel;

namespace AirportTerminalMonitoring.WPF.Commands
{
    public class GenericAddCommand<T> : IUndoRedoCommand where T : class
    {
        private readonly ObservableCollection<T> _collection;
        private readonly IRepository<T> _repository;
        private readonly T _entity;

        public GenericAddCommand(ObservableCollection<T> collection, IRepository<T> repository, T entity)
        {
            _collection = collection;
            _repository = repository;
            _entity = entity;
        }

        public void Execute()
        {
            _repository.Add(_entity);
            _collection.Add(_entity);
        }

        public void Undo()
        {
            var idProperty = _entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                Guid id = (Guid)idProperty.GetValue(_entity);
                _repository.Delete(id);
                _collection.Remove(_entity);
            }
        }
    }
}
