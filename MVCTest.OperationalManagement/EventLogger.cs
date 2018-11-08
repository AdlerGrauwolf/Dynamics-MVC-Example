using System;
using System.Diagnostics;

using MVCTest.OperationalManagement.Extensions;

namespace MVCTest.OperationalManagement
{
    public class EventLogger : ILogger
    {
        private const string source = "Laureate.WebSite";

        public void Error(Exception ex)
        {
            string message = string.Format("Error ocurrido {0} detalle: {1}", DateTime.Now, ex.Build());
            EventLog.WriteEntry(source, message, EventLogEntryType.Error);
        }

        public void Error(string error)
        {
            EventLog.WriteEntry(source, error, EventLogEntryType.Error);
        }

        public void Infomacion(string mensaje)
        {
            EventLog.WriteEntry(source, mensaje, EventLogEntryType.Information);
        }
    }
}
