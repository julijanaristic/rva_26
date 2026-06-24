using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Statistics.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Interfaces
{
    public interface IStatisticsService
    {
        //najveci broj putnika i prosecni broj kasnjenja po terminalu
        Dictionary<string, PassengerDelayStats> GetPassengerAndDelayStats(
                   Dictionary<string, List<TerminalActivity>> data);

        //maksimalno prosecno vreme cekanja i datum kad je bilo
        Dictionary<string, MaxWaitTimeStats> GetMaxWaitTimeStats(
            Dictionary<string, List<TerminalActivity>> data);

        //koliko puta su terminali bili zatvoreni
        int GetClosedCount(
            Dictionary<string, List<TerminalActivity>> data);

    }
}
