using System;
using System.Collections.Generic;
using D3D = SlimDX.Direct3D9;

namespace Irelia.Render
{
    public class RenderTarget : IRenderTarget
    {
        public D3D.Surface TargetSurface { get; set; }
        public D3D.Surface DepthStencilSurface { get; set; }
        public float LastFps { get; private set; }
        public bool ClearBackGround { get; set; }
        public D3D.ClearFlags ClearOptions { get; set; }
        public Color ClearColor { get; set; }
        public Size Size { get; set; }

        private uint frameCount;
        private readonly Timer timer = new Timer();
        private long lastTime;

        public RenderTarget()
        {
            ClearBackGround = true;
            ClearOptions = D3D.ClearFlags.Target | D3D.ClearFlags.ZBuffer;
            ClearColor = Color.Black;

            this.timer.Start();
        }

        void IRenderTarget.OnRender()
        {
            OnRender();
        }

        protected virtual void OnRender()
        {
            ++this.frameCount;

            var now = this.timer.Elapsed;

            if (now - this.lastTime > 1000)
            {
                LastFps = (float)this.frameCount / (float)(now - this.lastTime) * 1000.0f;
                this.lastTime = now;
                this.frameCount = 0;
            }
        }
    }
}
