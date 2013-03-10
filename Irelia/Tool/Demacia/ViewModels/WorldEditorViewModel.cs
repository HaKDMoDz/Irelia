using System.Windows.Input;
using Demacia.Command;
using Demacia.Models;
using Irelia.Render;
using Demacia.Services;

namespace Demacia.ViewModels
{
    public sealed class WorldEditorViewModel : ViewModelBase
    {
        #region Private Fields
        private Vector2 mousePos;
        #endregion

        #region Properties
        public RenderViewModel RenderViewModel { get; private set; }
        public WorldEditor WorldEditor { get; private set; }
        public SceneManager SceneManager { get; private set; }
        #endregion

        public DelegateCommand SaveCommand { get; private set; }

        public WorldEditorViewModel(WorldAsset worldAsset, Framework framework)
        {
            SceneManager = new SceneManager(framework.Device, framework.AssetManager);

            WorldEditor = new WorldEditor(worldAsset.World, framework.AssetManager);
            var terrain = new Terrain(framework.Device, framework.AssetManager, 128, 128, 20, new string[] { "Test/water.bmp", "Test/slime2.bmp", "Test/slime1.bmp" });
            SceneManager.AddRenderable(terrain);
            
            SceneManager.World = worldAsset.World;

            SceneManager.Camera.EyePos = new Vector3(0.0f, terrain.GetHeight(0, 0) + 10.0f, 0.0f);
            SceneManager.Camera.LookAt = new Vector3(100.0f, 0.0f, 100.0f);
            SceneManager.Camera.UpVec = Vector3.UnitY;
            SceneManager.Camera.FovY = new Radian(MathUtils.PI / 4);
            SceneManager.Camera.SetPlaneDistance(1.0f, 10000.0f);

            RenderViewModel = new RenderViewModel(framework, SceneManager);
            RenderViewModel.MouseMove += RenderViewModel_MouseMove;

            SaveCommand = new DelegateCommand(() =>
                {
                    worldAsset.World.Save(worldAsset.FullPath);
                    StatusBarService.StatusText = "World saved: " + worldAsset.ShortName;
                });
        }

        private void RenderViewModel_MouseMove(object sender, RenderViewModel.MouseMoveEventArgs e)
        {
            if (e.Args.RightButton == MouseButtonState.Pressed)
            {
                float scale = 0.001f;
                var delta = e.Pos - this.mousePos;

                SceneManager.Camera.Yaw(new Radian(-delta.x * scale));
                SceneManager.Camera.Pitch(new Radian(-delta.y * scale));
            }

            this.mousePos = e.Pos;
        }
    }
}
