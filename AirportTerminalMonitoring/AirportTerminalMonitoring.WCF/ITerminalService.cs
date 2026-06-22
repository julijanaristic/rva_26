using AirportTerminalMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.WCF
{
    [ServiceContract]
    public interface ITerminalService
    {
        [OperationContract]
        List<TerminalActivity> GetAllActivities();

        [OperationContract]
        List<AirportTerminal> GetAllTerminals();
    }
}
