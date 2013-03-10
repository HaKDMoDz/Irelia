using System.Collections.Generic;
using Demacia.Command;
using Demacia.Services;
using Demacia.ViewModels;
using Demacia.Views;
using Irelia.Render;
using System.Windows.Input;

namespace Demacia.Models
{
    public sealed class MeshAsset : Asset
    {
        private Mesh Mesh
        {
            get
            {
                if (this.mesh == null)
                {
                    this.mesh = this.framework.AssetManager.Load(Name) as Mesh;
                    OnPropertyChanged("IsLoaded");
                }
                return this.mesh;
            }
        }
        private Mesh mesh;

        public override ICommand OpenCommand { get { return this.openCommand; } }
        private readonly DelegateCommand openCommand;

        public override bool IsLoaded
        {
            get { return this.mesh != null; }
        }

        public MeshAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Mesh), name, framework, thumbnailMgr)
        {
            this.openCommand = new DelegateCommand(OpenEditor);
            Operations.Add(new Operation() { Name = "Edit Using Mesh Editor...", Command = this.openCommand });
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var sceneManager = new SceneManager(this.framework.Device, this.framework.AssetManager);
            sceneManager.AddRenderable(new MeshNode(this.framework.Device, Mesh));
            sceneManager.LocateCameraLookingMesh(Mesh);
            return sceneManager.Render(this.framework.Renderer, renderTarget, false);
        }

        private void OpenEditor()
        {
            var doc = new MeshEditorView()
            {
                Title = "Mesh Editor: " + Name,
                DataContext = new MeshEditorViewModel(Mesh, this.framework)
            };
            DocumentService.ShowDocument(doc, false);
        }
    }
}
