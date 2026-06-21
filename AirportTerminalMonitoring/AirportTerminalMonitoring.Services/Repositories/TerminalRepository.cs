using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTerminalMonitoring.Services.Repositories
{
    public class TerminalRepository : IRepository<AirportTerminal>
    {
        private readonly List<AirportTerminal> _terminals = new List<AirportTerminal>();

        public IEnumerable<AirportTerminal> GetAll()
        {
            return _terminals;
        }

        public AirportTerminal GetById(Guid id)
        {
            return _terminals.FirstOrDefault(t => t.Id == id);
        }

        public void Add(AirportTerminal entity)
        {
            _terminals.Add(entity);
        }

        public void Delete(Guid id)
        {
            var terminal = GetById(id);

            if (terminal != null) 
                _terminals.Remove(terminal);
        }

        public void Update(AirportTerminal entity)
        {
            var existing = GetById(entity.Id);

            if (existing == null)
                return;

            existing.Code = entity.Code;
            existing.AirportName = entity.AirportName;
            existing.GateCount = entity.GateCount;
            existing.DirectExit = entity.DirectExit;    
        }

    }
}
