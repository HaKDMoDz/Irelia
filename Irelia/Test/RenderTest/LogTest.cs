using System;
using System.Diagnostics;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class LogTest
    {
        private class MyTraceListener : TraceListener
        {
            public override void Write(string message)
            {
                LastWrittenMsg = message;
            }

            public override void WriteLine(string message)
            {
                LastWrittenMsg = message;
            }

            public string LastWrittenMsg { get; private set; }
        }

        private readonly MyTraceListener listener = new MyTraceListener();
        private TraceLevel traceLevel;

        [TestInitialize()]
        public void SetUp()
        {
            this.traceLevel = Log.TraceSwitch.Level;
            Trace.Listeners.Add(this.listener);
        }

        [TestCleanup()]
        public void TearDown()
        {
            Trace.Listeners.Remove(this.listener);
            Log.TraceSwitch.Level = this.traceLevel;
        }

        [TestMethod()]
        public void Msg_Test()
        {
            Log.TraceSwitch.Level = TraceLevel.Verbose;

            foreach (TraceLevel level in Enum.GetValues(typeof(TraceLevel)))
            {
                var msg = level.ToString() + ": log message";
                Log.Msg(level, msg);
                Assert.AreEqual("Render: " + msg, this.listener.LastWrittenMsg);
            }
        }

        [TestMethod()]
        public void Msg_Change_TraceLevel_Test()
        {
            Log.TraceSwitch.Level = TraceLevel.Error;

            Log.Msg(TraceLevel.Warning, "message will be ignored");
            Assert.IsNull(this.listener.LastWrittenMsg);

            var msg = "message will be written";
            Log.Msg(TraceLevel.Error, msg);
            Assert.AreEqual("Render: " + msg, this.listener.LastWrittenMsg);
        }
    }
}
