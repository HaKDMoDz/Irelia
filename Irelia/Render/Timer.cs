using System.Diagnostics;

namespace Irelia.Render
{
    public sealed class Timer
    {
        private readonly Stopwatch stopWatch = new Stopwatch();

        public void Start(bool reset = false)
        {
            if (reset && IsStarted)
                Stop(true);

            this.stopWatch.Start();
        }

        public void Stop(bool reset)
        {
            if (reset)
                this.stopWatch.Reset();
            else
                this.stopWatch.Stop();
        }

        public int Elapsed
        {
            get { return (int)this.stopWatch.ElapsedMilliseconds; }
        }

        public bool IsStarted
        {
            get { return this.stopWatch.IsRunning; }
        }
    }
}
