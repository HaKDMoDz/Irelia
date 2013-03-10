using SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class DarkenBorder : IRenderable
    {
        private readonly Device device;
        private readonly Shader shader;
        private TextureRenderTarget textureRenderTarget;

        public bool IsEnabled { get; set; }
        public Texture BorderTexture { get; set; }

        public DarkenBorder(Device device, AssetManager assetManager)
        {
            this.device = device;
            this.shader = assetManager.Load(@"Engine\DarkenBorder.fx") as Shader;
            IsEnabled = true;
        }

        #region Implements IRenderable
        bool IRenderable.Render(Camera camera, Light light)
        {
            if (IsEnabled == false)
                return true;

            if (this.textureRenderTarget == null ||
                this.textureRenderTarget.Size.Width != this.device.RenderTarget.Size.Width ||
                this.textureRenderTarget.Size.Height != this.device.RenderTarget.Size.Height)
            {
                this.textureRenderTarget = new TextureRenderTarget(this.device, this.device.RenderTarget.Size);
            }

            // Copy rendered buffer to our texture render target
            this.device.RawDevice.StretchRectangle(this.device.RenderTarget.TargetSurface, this.textureRenderTarget.TargetSurface, TextureFilter.None);

            this.device.RawDevice.SetRenderState(RenderState.ZEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.AlphaBlendEnable, false);

            this.device.RawDevice.VertexDeclaration = this.device.ScreenVertexDecl.RawDecl;

            if (this.device.RawDevice.SetStreamSource(0, this.device.ScreenVertexBuffer.RawBuffer, 0, this.device.ScreenVertexBuffer.ElementSize).IsFailure)
                return false;

            var windowSize = new Vector2(this.textureRenderTarget.Size.Width, this.textureRenderTarget.Size.Height);
            this.shader.Params["windowSize"].SetValue(windowSize);
            this.shader.Params["sceneMapTexture"].SetValue(this.textureRenderTarget.Texture);
            this.shader.Params["screenBorderFadeoutMapTexture"].SetValue(BorderTexture);

            bool ret = this.shader.RenderTechnique((pass) =>
            {
                return this.device.RawDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2).IsSuccess;
            }, "ScreenDarkenBorder");

            this.device.RawDevice.SetRenderState(RenderState.ZEnable, true);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, true);

            return ret;
        }
        #endregion
    }
}
