using System;
using System.Collections.Generic;
using System.Diagnostics;
using SlimDX;
using SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class RenderSystem
    {
        #region Fields
        private readonly Device device;
        private readonly SpriteRenderer spriteRenderer;
        private readonly IList<IRenderTarget> renderTargets = new List<IRenderTarget>();
        #endregion

        #region Properties
        public bool ManualPresent { get; set; }
        public RenderTarget PrimaryRenderTarget { get; private set; }
        #endregion

        public RenderSystem(Device device, SpriteRenderer spriteRenderer)
        {
            if (device == null)
                throw new ArgumentNullException("device");
            if (spriteRenderer == null)
                throw new ArgumentNullException("spriteRenderer");

            this.device = device;
            this.spriteRenderer = spriteRenderer;
            PrimaryRenderTarget = new RenderTarget();
        }

        #region Public methods
        public bool Render(IRenderTarget renderTarget, IList<IRenderable> renderables, IList<ISprite> sprites, Camera camera, Light light, bool present)
        {
            if (camera == null || light == null)
                throw new ArgumentNullException("Argument camera or light is null");

            bool renderSuccess = false;
            try
            {
                // Save primary
                PrimaryRenderTarget.TargetSurface = this.device.RawDevice.GetRenderTarget(0);
                PrimaryRenderTarget.DepthStencilSurface = this.device.RawDevice.DepthStencilSurface;

                // Render this render target
                renderSuccess = RenderToRenderTarget(renderTarget, renderables, sprites, present, camera, light);
            }
            finally
            {
                // Restore primary
                this.device.SetRenderTarget(PrimaryRenderTarget);
            }

            return renderSuccess;
        }
        #endregion

        #region Private methods
        private bool RenderToRenderTarget(IRenderTarget target, IList<IRenderable> renderables, IList<ISprite> sprites, bool doPresent, Camera camera, Light light)
        {
            this.device.SetRenderTarget(target);

            if (target.ClearBackGround)
                this.device.RawDevice.Clear(target.ClearOptions, target.ClearColor.ToArgb(), 1.0f, 0);

            Result result = this.device.RawDevice.BeginScene();
            if (result.IsFailure)
            {
                Log.Msg(TraceLevel.Error, this, "BeginScene failed, " + result.ToString());
                return false;
            }

            try
            {
                if (renderables != null && renderables.Count > 0)
                {
                    foreach (var renderable in renderables)
                    {
                        renderable.Render(camera, light);
                    }
                }

                if (sprites != null && sprites.Count > 0)
                {
                    this.spriteRenderer.RawSprite.Begin(SpriteFlags.AlphaBlend);
                    foreach (var sprite in sprites)
                    {
                        sprite.Render(this.spriteRenderer);
                    }
                    this.spriteRenderer.RawSprite.End();
                }
            }
            finally
            {
                this.device.RawDevice.EndScene();
            }

            if (doPresent)
                this.device.RawDevice.Present();

            target.OnRender();

            return true;
        }
        #endregion
    }
}
