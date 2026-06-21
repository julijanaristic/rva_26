using AirportTerminalMonitoring.Domain.Enums;

namespace AirportTerminalMonitoring.Services.Simulation
{
    public class TerminalStateSimulationService
    {
        public TerminalState GetNextState(TerminalState current)
        {
            switch (current)
            {
                case TerminalState.Operational:
                    return TerminalState.Congested;
                case TerminalState.Congested:
                    return TerminalState.Delayed;
                case TerminalState.Delayed:
                    return TerminalState.Closed;
                case TerminalState.Closed:
                    return TerminalState.Operational;
                default:
                    return TerminalState.Operational;
            }
        }
    }
}
