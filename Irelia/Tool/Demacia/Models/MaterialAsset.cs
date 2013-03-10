using System.Collections.Generic;
using System.Windows.Input;
using Demacia.Command;
using Demacia.Services;
using Demacia.ViewModels;
using Demacia.Views;
using Irelia.Render;

namespace Demacia.Models
{
    public sealed class MaterialAsset : Asset
    {
        public Material Material
        {
            get
            {
                if (this.material == null)
                {
                    this.material = this.framework.AssetManager.Load(Name) as Material;
                    OnPropertyChanged("IsLoaded");
                }
                return this.material;
            }
        }
        private Material material;

        public override ICommand OpenCommand { get { return this.openCommand; } }
        private readonly DelegateCommand openCommand;

        public override bool IsLoaded
        {
            get { return this.material != null; }
        }

        public MaterialAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Material), name, framework, thumbnailMgr)
        {
            this.openCommand = new DelegateCommand(OpenEditor);
            Operations.Add(new Operation() { Name = "Edit Using Material Editor...", Command = OpenCommand });
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var mesh = Mesh.CreateSphere(this.framework.Device, this.framework.AssetManager, Material, 5.0f, 80, 80);
            var sceneManager = new SceneManager(this.framework.Device, this.framework.AssetManager);
            sceneManager.AddRenderable(new MeshNode(this.framework.Device, mesh));
            sceneManager.LocateCameraLookingMesh(mesh);
            return sceneManager.Render(this.framework.Renderer, renderTarget, false);
        }

        private void OpenEditor()
        {
            var doc = new MaterialEditorView()
            {
                Title = "Material Editor: " + Name,
                DataContext = new MaterialEditorViewModel(this, this.framework)
            };
            DocumentService.ShowDocument(doc, false);
        }
    }
}
