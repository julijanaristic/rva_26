using AirportTerminalMonitoring.Services.Interfaces;
using System;
using System.IO;

namespace AirportTerminalMonitoring.Services.Logging
{
    public class FileLogger : ILoggerService
    {
        private readonly string _path = "logs.txt";

        public void Log(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(_path, logMessage + Environment.NewLine);
        }
    }
}
