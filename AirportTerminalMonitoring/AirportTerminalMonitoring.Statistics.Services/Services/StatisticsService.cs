using AirportTerminalMonitoring.Domain.Enums;
using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Statistics.Services.DTOs;
using AirportTerminalMonitoring.Statistics.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Services
{
    public class StatisticsService : IStatisticsService
    {
        public Dictionary<string, PassengerDelayStats> GetPassengerAndDelayStats(
       Dictionary<string, List<TerminalActivity>> data)
        {
            var result = new Dictionary<string, PassengerDelayStats>();
            var countTracker = new Dictionary<string, int>();

            foreach (var entry in data)
            {
                foreach (var activity in entry.Value)
                {
                    string key = activity.TerminalId.ToString();

                    if (!result.ContainsKey(key))
                    {
                        result[key] = new PassengerDelayStats
                        {
                            MaxPassengers = activity.PassengerCount,
                            AvgDelays = activity.NumberOfDelays
                        };
                        countTracker[key] = 1;
                    }
                    else
                    {
                        countTracker[key]++;

                        result[key].MaxPassengers = Math.Max(result[key].MaxPassengers, activity.PassengerCount);

                        double totalDelays = (result[key].AvgDelays * (countTracker[key] - 1)) + activity.NumberOfDelays;
                        result[key].AvgDelays = totalDelays / countTracker[key];
                    }
                }
            }

            return result;
        }

        public Dictionary<string, MaxWaitTimeStats> GetMaxWaitTimeStats(
            Dictionary<string, List<TerminalActivity>> data)
        {
            var result = new Dictionary<string, MaxWaitTimeStats>();

            foreach (var entry in data)
            {
                foreach (var activity in entry.Value)
                {
                    string key = activity.TerminalId.ToString();

                    if (!result.ContainsKey(key))
                    {
                        result[key] = new MaxWaitTimeStats
                        {
                            MaxWaitTime = activity.AverageWaitTime,
                            Date = activity.CollectionTime
                        };
                    }
                    else if (activity.AverageWaitTime > result[key].MaxWaitTime)
                    {
                        result[key].MaxWaitTime = activity.AverageWaitTime;
                        result[key].Date = activity.CollectionTime;
                    }
                }
            }

            return result;
        }

        public Dictionary<string, int> GetClosedCount(
            Dictionary<string, List<TerminalActivity>> data)
        {
            var result = new Dictionary<string, int>();

            foreach (var entry in data)
            {
                foreach (var activity in entry.Value)
                {
                    if (activity.State != TerminalState.Closed)
                        continue;

                    string key = activity.TerminalId.ToString();

                    if (!result.ContainsKey(key))
                        result[key] = 0;

                    result[key]++;
                }
            }

            return result;
        }
    }
}
