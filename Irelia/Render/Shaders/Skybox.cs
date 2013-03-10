using SlimDX.Direct3D9;

namespace Irelia.Render
{
    public sealed class Skybox : DisposableObject, IRenderable
    {            
        private readonly Device device;
        private readonly Shader shader;

        public CubeTexture SkyTexture
        {
            get;
            set;
        }

        public Color AmbientColor { get; set; }

        public Skybox(Device device, AssetManager assetManager)
        {
            this.device = device;
            this.shader = assetManager.Load(@"Engine\Skybox.fx") as Shader;
            AmbientColor = Color.White;
        }

        #region Implements IRenderable
        bool IRenderable.Render(Camera camera, Light light)
        {
            this.device.RawDevice.SetRenderState(RenderState.ZEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, false);
            this.device.RawDevice.SetRenderState(RenderState.AlphaBlendEnable, false);

            this.device.RawDevice.VertexDeclaration = this.device.ScreenVertexDecl.RawDecl;

            if (this.device.RawDevice.SetStreamSource(0, this.device.ScreenVertexBuffer.RawBuffer, 0, this.device.ScreenVertexBuffer.ElementSize).IsFailure)
                return false;

            this.shader.Params["m_ViewInv"].SetValue(camera.ViewMatrix.Invert());
            this.shader.Params["diffuseTexture"].SetValue(SkyTexture);
            this.shader.Params["ambientColor"].SetValue(AmbientColor);
            this.shader.Params["scale"].SetValue(1.0f);
            this.shader.Params["aspectRatio"].SetValue(camera.AspectRatio);

            bool ret = this.shader.RenderTechnique((pass) =>
                {
                    return this.device.RawDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2).IsSuccess;
                }, "Skybox");

            this.device.RawDevice.SetRenderState(RenderState.ZEnable, true);
            this.device.RawDevice.SetRenderState(RenderState.ZWriteEnable, true);

            return ret;
        }
        #endregion

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
