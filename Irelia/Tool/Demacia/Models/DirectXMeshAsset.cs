using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Demacia.Command;
using Demacia.Services;
using Demacia.ViewModels;
using Demacia.Views;
using Irelia.Render;

namespace Demacia.Models
{
    public sealed class DirectXMeshAsset : Asset
    {
        private Mesh mesh;
        private Mesh Mesh
        {
            get
            {
                if (this.mesh == null)
                {
                    this.mesh = this.framework.AssetManager.Load(FullPath, null) as Mesh;
                    OnPropertyChanged("IsLoaded");
                }
                return this.mesh;
            }
        }

        private readonly DelegateCommand importCommand;
        public override ICommand OpenCommand { get { return this.importCommand; } }

        public override bool IsLoaded
        {
            get { return this.mesh != null; }
        }

        public DirectXMeshAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(DirectXMesh), name, framework, thumbnailMgr)
        {
            this.importCommand = new DelegateCommand(ShowImportDialog);
            Operations.Add(new Operation() { Name = "Import...", Command = this.importCommand });
        }
        
        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var sceneManager = new SceneManager(this.framework.Device, this.framework.AssetManager);
            sceneManager.AddRenderable(new MeshNode(this.framework.Device, Mesh));
            sceneManager.LocateCameraLookingMesh(Mesh);
            return sceneManager.Render(this.framework.Renderer, renderTarget, false);
        }

        private void ShowImportDialog()
        {
            var dlg = new ImportMeshView()
            {
                DataContext = new ImportMeshViewModel(this.framework, Mesh, Path.GetDirectoryName(FullPath))
            };

            dlg.ShowDialog();
        }
    }
}
