using System;
using System.Diagnostics;

namespace Demacia.Models
{
    public class LogData
    {
        public DateTime Time { get; private set; }
        public TraceLevel Level { get; private set; }
        public string Message { get; private set; }
        public string Category { get; private set; }

        public LogData(DateTime dateTime, TraceLevel level, string message, string category)
        {
            Time = dateTime;
            Level = level;
            Message = message;
            Category = category;
        }
    }
}
