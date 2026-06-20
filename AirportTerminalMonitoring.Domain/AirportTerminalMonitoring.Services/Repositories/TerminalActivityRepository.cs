using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTerminalMonitoring.Services.Repositories
{
    public class TerminalActivityRepository : IRepository<TerminalActivity>
    {
        private readonly List<TerminalActivity> _activities = new List<TerminalActivity>();

        public IEnumerable<TerminalActivity> GetAll()
        {
            return _activities;
        }

        public TerminalActivity GetById(Guid id)
        {
            return _activities.FirstOrDefault(a => a.Id == id);
        }

        public void Add(TerminalActivity entity)
        {
            _activities.Add(entity);
        }

        public void Update(TerminalActivity entity)
        {
            var existing = GetById(entity.Id);

            if (existing == null)
                return;

            existing.TerminalId = entity.TerminalId;
            existing.CollectionTime = entity.CollectionTime;
            existing.PassengerCount = entity.PassengerCount;    
            existing.AverageWaitTime = entity.AverageWaitTime;
            existing.NumberOfDelays = entity.NumberOfDelays;
            existing.State = entity.State;
        }

        public void Delete(Guid id)
        {
            var activity = GetById(id);

            if (activity != null) 
                _activities.Remove(activity);
        }

    }
}
