using System.Collections.Generic;
using System.IO;
using Irelia.Render;
using System.Windows.Input;
using D3D = SlimDX.Direct3D9;

namespace Demacia.Models
{
    public sealed class TextureAsset : Asset
    {
        public Texture Texture
        {
            get 
            {
                if (this.texture == null)
                {
                    this.texture = this.framework.AssetManager.Load(Name) as Texture;
                    OnPropertyChanged("IsLoaded");
                }
                return this.texture;
            }
        }
        private Texture texture;

        public D3D.ImageInformation Information { get; private set; }

        public override ICommand OpenCommand { get { return null; } }
        public override bool IsLoaded
        {
            get { return this.texture != null; }
        }

        #region Inner class, FlatTexturePlane
        private class FlatTexturePlane : ManualRenderable<TexturedVertex>
        {
            private readonly Shader shader;
            private readonly Texture texture;

            public FlatTexturePlane(Device device, AssetManager assetManager, Texture texture)
                : base(device, device.TextureVertexDecl)
            {
                this.shader = assetManager.Load(@"Engine\FlatTexture.fx") as Shader;
                this.texture = texture;

                var vertices = new[]
                {
                    new TexturedVertex() { Position = new Vector3(1.0f, -1.0f, 0),     UV = new Vector2(1, 1) },
                    new TexturedVertex() { Position = new Vector3(-1.0f, -1.0f, 0),    UV = new Vector2(0, 1) },
                    new TexturedVertex() { Position = new Vector3(1.0f, 1.0f, 0),      UV = new Vector2(1, 0) },
                    new TexturedVertex() { Position = new Vector3(-1.0f, 1.0f, 0),     UV = new Vector2(0, 0) }
                };
                AddVertex(vertices);
            }

            protected override bool Render(Camera camera, Light light)
            {
                var shaderParams = this.shader.Params;

                if (shaderParams["gTexture"].SetValue(this.texture) == false)
                    return false;

                if (shaderParams["gWvpXf"].SetValue(Matrix4.Identity) == false)
                    return false;

                return this.shader.RenderTechnique((pass) => RenderVertices(OperationType.TriangleStrip), "SimpleEffect");
            }
        }
        #endregion

        public TextureAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Texture), name, framework, thumbnailMgr)
        {
            if (File.Exists(FullPath))
                Information = D3D.ImageInformation.FromFile(FullPath);
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var plane = new FlatTexturePlane(this.framework.Device, this.framework.AssetManager, Texture);

            return this.framework.Renderer.Render(renderTarget, new List<IRenderable>() { plane }, null, new Camera(), new Light(), false);
        }
    }
}
