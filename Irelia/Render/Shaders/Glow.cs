using SlimDX.Direct3D9;
using System;

namespace Irelia.Render
{
    public sealed class Glow : IRenderable
    {
        private readonly Device device;
        private readonly Shader shader;
        private TextureRenderTarget sceneMapRenderTarget;
        private TextureRenderTarget radialSceneMapRenderTarget;
        private TextureRenderTarget downsampleMapRenderTarget;
        private TextureRenderTarget blurMap1RenderTarget;
        private TextureRenderTarget blurMap2RenderTarget;

        public bool IsEnabled { get; set; }
        public float RadialBlurScaleFactor { get; set; }
        public float BlurWidth { get; set; }
        public float GlowIntensity { get; set; }
        public float HighlightIntensity { get; set; }

        public Glow(Device device, AssetManager assetManager)
        {
            this.device = device;
            this.shader = assetManager.Load(@"Engine\Glow.fx") as Shader;
            
            IsEnabled = true;
            RadialBlurScaleFactor = -0.004f;
            BlurWidth = 8.0f;
            GlowIntensity = 0.7f;
            HighlightIntensity = 0.4f;
        }

        #region Implements IRenderable
        bool IRenderable.Render(Camera camera, Light light)
        {
            if (IsEnabled == false)
                return true;

            var finalRenderTarget = this.device.RenderTarget;

            CreateTextureRenderTargets(finalRenderTarget.Size);
            CopyRenderTarget(finalRenderTarget, this.sceneMapRenderTarget);

            this.device.RawDevice.SetRenderState(RenderState.ZEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.AlphaBlendEnable, false);

            this.device.RawDevice.VertexDeclaration = this.device.ScreenVertexDecl.RawDecl;

            if (this.device.RawDevice.SetStreamSource(0, this.device.ScreenVertexBuffer.RawBuffer, 0, this.device.ScreenVertexBuffer.ElementSize).IsFailure)
                return false;

            var windowSize = new Vector2(finalRenderTarget.Size.Width, finalRenderTarget.Size.Height);
            this.shader.Params["windowSize"].SetValue(windowSize);
            this.shader.Params["radialBlurScaleFactor"].SetValue(RadialBlurScaleFactor);
            this.shader.Params["sceneMapTexture"].SetValue(this.sceneMapRenderTarget.Texture);
            this.shader.Params["radialSceneMapTexture"].SetValue(this.radialSceneMapRenderTarget.Texture);
            this.shader.Params["downsampleMapTexture"].SetValue(this.downsampleMapRenderTarget.Texture);
            this.shader.Params["blurMap1Texture"].SetValue(this.blurMap1RenderTarget.Texture);
            this.shader.Params["blurMap2Texture"].SetValue(this.blurMap2RenderTarget.Texture);
            this.shader.Params["blurWidth"].SetValue(BlurWidth);
            this.shader.Params["glowIntensity"].SetValue(GlowIntensity);
            this.shader.Params["highlightIntensity"].SetValue(HighlightIntensity);

            bool ret = this.shader.RenderTechnique(RenderPass, "ScreenGlow");

            this.device.RawDevice.SetRenderState(RenderState.ZEnable, true);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, true);

            return ret;
        }

        private bool RenderPass(int pass)
        {
            var finalRenderTarget = this.device.RenderTarget;

            try
            {
                if (pass == 0)
                    this.device.SetRenderTarget(this.radialSceneMapRenderTarget);
                else if (pass == 1)
                    this.device.SetRenderTarget(this.downsampleMapRenderTarget);
                else if (pass == 2)
                    this.device.SetRenderTarget(this.blurMap1RenderTarget);
                else if (pass == 3)
                    this.device.SetRenderTarget(this.blurMap2RenderTarget);
                else
                    this.device.SetRenderTarget(finalRenderTarget);

                return this.device.RawDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2).IsSuccess;
            }
            finally
            {
                this.device.SetRenderTarget(finalRenderTarget);
            }
        }

        private void CopyRenderTarget(IRenderTarget source, IRenderTarget target)
        {
            this.device.RawDevice.StretchRectangle(source.TargetSurface, target.TargetSurface, TextureFilter.None);
        }

        private void CreateTextureRenderTargets(Size screenSize)
        {
            var quarterSize = new Size(screenSize.Width / 4, screenSize.Height / 4);
            quarterSize.Width = Math.Max(1, quarterSize.Width);
            quarterSize.Height = Math.Max(1, quarterSize.Height);

            CreateTextureRenderTargetIfRequired(ref this.sceneMapRenderTarget, screenSize);
            CreateTextureRenderTargetIfRequired(ref this.radialSceneMapRenderTarget, screenSize);
            CreateTextureRenderTargetIfRequired(ref this.downsampleMapRenderTarget, quarterSize);
            CreateTextureRenderTargetIfRequired(ref this.blurMap1RenderTarget, quarterSize);
            CreateTextureRenderTargetIfRequired(ref this.blurMap2RenderTarget, quarterSize);
        }

        private void CreateTextureRenderTargetIfRequired(ref TextureRenderTarget renderTarget, Size size)
        {
            if (renderTarget == null ||
                renderTarget.Size.Width != size.Width ||
                renderTarget.Size.Height != size.Height)
            {
                renderTarget = new TextureRenderTarget(this.device, size);
            }
        }
        #endregion
    }
}
