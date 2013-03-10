using System.Diagnostics;
using Log = Irelia.Render.Log;
using System;

namespace Demacia.Services
{
    public class LogService
    {
        private static readonly string category = "Demacia";

        public static TraceSwitch TraceSwitch { get; private set; }

        static LogService()
        {
            TraceSwitch = new TraceSwitch(category, "Trace switch for demacia tool")
            {
                Level = TraceLevel.Verbose
            };

            Log.Logged += ((o, e) => LogService.Logged(o, e));
        }

        public static event EventHandler<Log.LoggedEventArgs> Logged = delegate { };

        public static void Msg(TraceLevel level, object reporter, string msg)
        {
            Msg(level, reporter.ToString() + ": " + msg);
        }

        public static void Msg(TraceLevel level, string msg)
        {
            Trace.WriteIf(level <= TraceSwitch.Level, msg, category);
            Logged(null, new Log.LoggedEventArgs(level, msg, category));
        }
    }
}
