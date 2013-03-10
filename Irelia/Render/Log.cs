using System.Diagnostics;
using System;

namespace Irelia.Render
{
    public class Log
    {
        private static readonly string category = "Render";

        public class LoggedEventArgs : EventArgs
        {
            public TraceLevel Level { get; private set; }
            public string Message { get; private set; }
            public string Category { get; private set; }

            public LoggedEventArgs(TraceLevel level, string message, string category)
            {
                Level = level;
                Message = message;
                Category = category;
            }
        }

        public static event EventHandler<LoggedEventArgs> Logged = delegate { };

        public static TraceSwitch TraceSwitch { get; private set; }

        static Log()
        {
            TraceSwitch = new TraceSwitch(category, "Trace switch for irelia engine")
            {
                Level = TraceLevel.Verbose
            };
        }

        public static void Msg(TraceLevel level, object reporter, string msg)
        {
            Msg(level, reporter.ToString() + ": " + msg);
        }

        public static void Msg(TraceLevel level, string msg)
        {
            Trace.WriteLineIf(level <= TraceSwitch.Level, msg, category);
            Logged(null, new LoggedEventArgs(level, msg, category));
        }

    }
}
