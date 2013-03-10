using System.Collections.Generic;
using System.IO;
using Irelia.Render;
using System.Windows.Input;
using Demacia.Services;
using Demacia.Command;
using Demacia.ViewModels;
using Demacia.Views;

namespace Demacia.Models
{
    public sealed class ShaderAsset : Asset
    {
        public Shader Shader
        {
            get
            {
                if (this.shader == null)
                {
                    this.shader = this.framework.AssetManager.Load(Name) as Shader;
                    OnPropertyChanged("IsLoaded");
                }
                return this.shader;
            }
        }
        private Shader shader;

        public override ICommand OpenCommand { get { return this.openCommand; } }
        public override bool IsLoaded
        {
            get { return this.shader != null; }
        }

        private DelegateCommand openCommand;

        public ShaderAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Shader), name, framework, thumbnailMgr)
        {
            this.openCommand = new DelegateCommand(OpenEditor);
            Operations.Add(new Operation() { Name = "Edit Using Shader Editor...", Command = OpenCommand });
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var material = new Material(Shader + " Material")
            {
                Shader = Shader,
                DiffuseTexture = this.framework.AssetManager.DefaultTexture
            };

            var mesh = Mesh.CreateSphere(this.framework.Device, this.framework.AssetManager, material, 5.0f, 80, 80);
            var sceneManager = new SceneManager(this.framework.Device, this.framework.AssetManager);
            sceneManager.AddRenderable(new MeshNode(this.framework.Device, mesh));
            sceneManager.LocateCameraLookingMesh(mesh);
            return sceneManager.Render(this.framework.Renderer, renderTarget, false);
        }

        private void OpenEditor()
        {
            var doc = new ShaderEditorView()
            {
                Title = "Shader Editor: " + Name,
                DataContext = new ShaderEditorViewModel(Shader)
            };
            DocumentService.ShowDocument(doc, false);
        }
    }
}
