using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sys = System.Threading;

namespace RenderTest
{
    [TestClass()]
    public class TimerTest
    {
        [TestMethod()]
        public void Timer_Constructor_Test()
        {
            var timer = new Timer();
            Assert.AreEqual(0, timer.Elapsed);
        }
        
        [TestMethod()]
        public void Timer_Stop_Test()
        {
            var timer = new Timer();
            timer.Start();
            Sys.Thread.Sleep(20);
            Assert.AreNotEqual(0, timer.Elapsed);
            
            timer.Stop(false);
            Assert.AreNotEqual(0, timer.Elapsed);

            timer.Start(false);
            Assert.AreNotEqual(0, timer.Elapsed);

            timer.Stop(true);
            Assert.AreEqual(0, timer.Elapsed);

            // Start again
            timer.Start();
            Sys.Thread.Sleep(10);
            timer.Start(true);
            Assert.AreEqual(0, timer.Elapsed);
        }
    }
}
