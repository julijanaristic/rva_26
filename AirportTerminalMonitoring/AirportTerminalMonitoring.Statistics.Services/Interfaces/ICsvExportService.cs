using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTerminalMonitoring.Statistics.Services.Interfaces
{
    public interface ICsvExportService
    {
        void Export(string content, string filePath);
    }
}
