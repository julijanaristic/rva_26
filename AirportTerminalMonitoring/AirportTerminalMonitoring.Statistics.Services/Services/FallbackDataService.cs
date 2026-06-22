using AirportTerminalMonitoring.Domain.Enums;
using AirportTerminalMonitoring.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AirportTerminalMonitoring.Statistics.Services.Services
{
    public class FallbackDataService
    {
        private static readonly string BASE_PATH = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory, // tvoj bin/Debug folder
    @"..\..\..\..\..\AirportTerminalMonitoring.WPF\bin\Debug\net8.0-windows7.0\"
);

        private static string TERMINALS_FILE => Path.GetFullPath(Path.Combine(BASE_PATH, "terminals.xml"));
        private static string ACTIVITIES_FILE => Path.GetFullPath(Path.Combine(BASE_PATH, "activities.xml"));

        public List<TerminalActivity> GetDefaultActivities()
        {
            // 1. Pokušaj da učitaš prave podatke koje je koleginica unela
            if (File.Exists(ACTIVITIES_FILE))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TerminalActivity>));
                    using (FileStream fs = new FileStream(ACTIVITIES_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        return (List<TerminalActivity>)serializer.Deserialize(fs);
                    }
                }
                catch (Exception)
                {
                    // Ako je fajl privremeno zaključan ili prazan, vrati tvoje hardkodovane podatke
                    return GetHardcodedActivities();
                }
            }

            // 2. Ako fajl još uvek ne postoji, vrati hardkodovane podatke
            return GetHardcodedActivities();
        }

        public List<AirportTerminal> GetDefaultTerminals()
        {
            if (File.Exists(TERMINALS_FILE))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<AirportTerminal>));
                    using (FileStream fs = new FileStream(TERMINALS_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        return (List<AirportTerminal>)serializer.Deserialize(fs);
                    }
                }
                catch (Exception)
                {
                    return new List<AirportTerminal>(); // ili tvoje hardkodovane terminale ako ih imaš
                }
            }

            return new List<AirportTerminal>();
        }

        private List<TerminalActivity> GetHardcodedActivities()
        {
            Guid terminal1 = Guid.NewGuid();
            Guid terminal2 = Guid.NewGuid();

            return new List<TerminalActivity>
            {
                new TerminalActivity
                {
                    Id = Guid.NewGuid(),
                    TerminalId = terminal1,
                    CollectionTime = new DateTime(2026, 5, 25),
                    PassengerCount = 25000,
                    AverageWaitTime = 60,
                    NumberOfDelays = 100,
                    State = TerminalState.Operational
                },
                new TerminalActivity
                {
                    Id = Guid.NewGuid(),
                    TerminalId = terminal1,
                    CollectionTime = new DateTime(2026, 5, 26),
                    PassengerCount = 17000,
                    AverageWaitTime = 45,
                    NumberOfDelays = 80,
                    State = TerminalState.Congested
                },
                new TerminalActivity
                {
                    Id = Guid.NewGuid(),
                    TerminalId = terminal2,
                    CollectionTime = new DateTime(2026, 5, 27),
                    PassengerCount = 22500,
                    AverageWaitTime = 80,
                    NumberOfDelays = 120,
                    State = TerminalState.Closed
                }
            };
        }
    }
}
