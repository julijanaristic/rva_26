using AirportTerminalMonitoring.Statistics.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Services
{
    public class CsvExportService : ICsvExportService
    {
        public void Export(string content, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
        }

    }
}
