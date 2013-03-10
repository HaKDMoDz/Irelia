using System.Collections.Generic;
using System.Windows.Controls;
using Demacia.Command;
using Demacia.Models;
using Demacia.Services;
using Irelia.Render;
using Microsoft.Windows.Controls.PropertyGrid;
using System.Windows.Input;

namespace Demacia.ViewModels
{
    public class MaterialEditorViewModel : ViewModelBase
    {
        #region Properties
        public RenderViewModel RenderViewModel { get; private set; }
        public MaterialEditor MaterialEditor { get; private set; }
        public IList<MenuItem> AdvancedOptions { get; private set; }
        #endregion

        #region Commands
        public DelegateCommand<PropertyItem> ResetCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        #endregion

        #region Fields
        private readonly MeshNode meshNode;
        private Vector2 mousePos;
        #endregion

        public MaterialEditorViewModel(MaterialAsset materialAsset, Framework framework)
        {
            MaterialEditor = new MaterialEditor(materialAsset.Material);
            ResetCommand = new DelegateCommand<PropertyItem>((item) => System.Windows.MessageBox.Show(item.ToString()));
            AdvancedOptions = new List<MenuItem>();
            AdvancedOptions.Add(new MenuItem() { Header = "Reset", Command = ResetCommand });
            SaveCommand = new DelegateCommand(() => 
                {
                    materialAsset.Material.Save(materialAsset.FullPath);
                    StatusBarService.StatusText = "Material saved: " + materialAsset.ShortName;
                });

            var mesh = Mesh.CreateSphere(framework.Device, framework.AssetManager, materialAsset.Material, 5.0f, 80, 80);
            this.meshNode = new MeshNode(framework.Device, mesh);

            var sceneManager = new SceneManager(framework.Device, framework.AssetManager);
            sceneManager.LocateCameraLookingMesh(mesh);
            sceneManager.AddRenderable(this.meshNode);
            
            RenderViewModel = new RenderViewModel(framework, sceneManager);
            RenderViewModel.MouseMove += RenderViewModel_MouseMove;
        }

        private void RenderViewModel_MouseMove(object sender, RenderViewModel.MouseMoveEventArgs e)
        {
            if (e.Args.RightButton == MouseButtonState.Pressed)
            {
                var delta = e.Pos - this.mousePos;
                this.meshNode.Orientation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, new Radian(-delta.x * 0.01f));
                this.meshNode.Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(-delta.y * 0.01f)) * this.meshNode.Orientation;
            }

            this.mousePos = e.Pos;
        }
    }
}
