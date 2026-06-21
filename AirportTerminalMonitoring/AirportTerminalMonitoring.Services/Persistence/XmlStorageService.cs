using AirportTerminalMonitoring.Domain.Models;
using AirportTerminalMonitoring.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AirportTerminalMonitoring.Services.Persistence
{
    public class XmlStorageService : IDataStorageService
    {
        private const string TERMINALS_FILE = "terminals.xml";
        private const string ACTIVITIES_FILE = "activities.xml";

        public List<AirportTerminal> LoadTerminals()
        {
            if (!File.Exists(TERMINALS_FILE))
                return new List<AirportTerminal>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<AirportTerminal>));

            using (FileStream fs = new FileStream(TERMINALS_FILE, FileMode.Open))
            {
                return (List<AirportTerminal>)serializer.Deserialize(fs);
            } 
        }

        public void SaveTerminals(IEnumerable<AirportTerminal> terminals)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AirportTerminal>));

            using (FileStream fs = new FileStream(TERMINALS_FILE, FileMode.Create))
            {
                serializer.Serialize(fs, terminals.ToList());
            }
        }

        public List<TerminalActivity> LoadActivities()
        {
            if (!File.Exists(ACTIVITIES_FILE))
                return new List<TerminalActivity>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<TerminalActivity>));

            using (FileStream fs = new FileStream(ACTIVITIES_FILE, FileMode.Open))
            {
                return (List<TerminalActivity>)serializer.Deserialize(fs);
            }

        }

        public void SaveActivities(IEnumerable<TerminalActivity> activities)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TerminalActivity>));

            using (FileStream fs = new FileStream(ACTIVITIES_FILE, FileMode.Create))
            {
                serializer.Serialize(fs, activities.ToList());
            }
        }

    }
}
