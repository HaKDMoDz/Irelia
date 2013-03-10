using System.Collections.Generic;
using System.Diagnostics;
using Input;
using Irelia.Gui;
using Irelia.Render;

namespace RocketCommander
{
    public class RocketCommanderGame : Game
    {
        #region Properties
        public GameCamera Camera
        {
            get { return this.camera; }
            set
            {
                this.camera.Deactivate();

                this.camera = value;

                this.sceneManager.Camera = this.camera;
                this.camera.AspectRatio = (float)Width / (float)Height;
                this.camera.FovY = FieldOfView;
                this.camera.Activate();
            }
        }

        public Radian FieldOfView
        {
            get { return new Radian(MathUtils.PI / 1.8f); }
        }
        #endregion

        #region Fields
        protected readonly Keyboard keyboard;
        protected readonly Mouse mouse;
        protected Layout debugLayout;
        protected readonly string fpsTextName = "FpsTextBox";
        
        private readonly List<Layout> layouts = new List<Layout>();
        private Screen screen;
        private GameCamera camera;
        private readonly RoamingRocketScene roamingRocketScene;
        #endregion

        public RocketCommanderGame(int width, int height)
            : base("RocketCommander", width, height)
        {
            this.framework.AssetManager.RegisterAssetFactory(new LayoutFactory());

            // Initialize input system
            this.keyboard = new Keyboard(form.Handle);
            this.mouse = new Mouse(form.Handle);

            this.camera = new FreeCamera(this.mouse, this.keyboard);
            this.roamingRocketScene = new RoamingRocketScene(this);
        }

        #region Overrides Game
        protected override void Initialize()
        {
            base.Initialize();

            this.mouse.MouseMove += MouseMove;
            this.mouse.MouseDown += MouseDown;
            this.mouse.MouseUp += MouseUp;

            var world = this.framework.AssetManager.Load(@"RocketCommander\RocketCommander.worldb") as World;
            this.sceneManager.World = world;

            AddDebugLayout();

            SetScreen(new MainMenuScreen(this, this.framework));
        }

        protected override void Update(int elapsed)
        {
            base.Update(elapsed);

            this.mouse.Update();
            this.camera.Update(elapsed);
            if (this.screen != null)
                this.screen.Update();
            if (this.roamingRocketScene.IsStarted)
                this.roamingRocketScene.Update(elapsed);

            var fpsText = this.debugLayout.GetElement(fpsTextName) as TextBlock;
            fpsText.Text = "FPS: " + this.framework.Renderer.PrimaryRenderTarget.LastFps;
        }
        #endregion

        #region Public Methods
        public bool AddLayout(Layout layout)
        {
            if (this.layouts.Contains(layout))
                return false;

            layout.Size = new Size(Width, Height);
            layout.GetVisualTree().ForEach(element => this.sceneManager.AddSprite(element));
            this.layouts.Add(layout);
            return true;
        }

        public bool RemoveLayout(Layout layout)
        {
            if (!this.layouts.Contains(layout))
                return false;

            layout.GetVisualTree().ForEach(element => this.sceneManager.RemoveSprite(element));
            return this.layouts.Remove(layout);
        }

        public void SetScreen(Screen screen)
        {
            if (this.screen != null)
                this.screen.Exit();

            this.screen = screen;
            if (this.screen != null)
                this.screen.Enter();
        }

        public void EnableRoamingRocketScene()
        {
            this.roamingRocketScene.Start();
        }

        public void DisableRoamingRocketScene()
        {
            this.roamingRocketScene.Stop();
        }
        #endregion

        private class RoamingRocketScene
        {
            public RoamingRocketScene(RocketCommanderGame game)
            {
                this.camera = new RoamingCamera(game.mouse, game.keyboard, game);
                this.rocket = new Rocket(game.framework);
                this.game = game;
            }

            public void Start()
            {
                if (IsStarted)
                    return;

                this.game.Camera = this.camera;
                this.game.sceneManager.AddRenderable(this.rocket.MeshNode);
                IsStarted = true;
            }

            public void Stop()
            {
                if (!IsStarted)
                    return;

                this.game.Camera = new FreeCamera(game.mouse, game.keyboard);
                this.game.sceneManager.RemoveRenderable(this.rocket.MeshNode);
                IsStarted = false;
            }

            public void Update(int elapsed)
            {
                var inFrontOfCameraPos = new Vector3(0, -1.33f, 2.5f);
                rocket.MeshNode.Position = inFrontOfCameraPos * game.Camera.ViewMatrix.Invert();

                rocket.MeshNode.Orientation = game.Camera.Orientation *
                    Quaternion.CreateFromAxisAngle(Vector3.UnitZ, new Radian(game.TotalGameTime / 8500.0f));
            }

            public bool IsStarted { get; private set; }

            private readonly GameCamera camera;
            private readonly Rocket rocket;
            private readonly RocketCommanderGame game;
        }

        #region Private Methods
        private void MouseMove(object sender, Mouse.MouseEventArgs args)
        {
            this.layouts.ForEach(l => l.InjectMouseMoveEvent(args.Position.x, args.Position.y));
        }

        private void MouseDown(object sender, Mouse.MouseButtonEventArgs args)
        {
            this.layouts.ForEach(l => l.InjectMouseDownEvent(args.Position.x, args.Position.y, args.ChangedButton.ToGuiButton()));
        }

        private void MouseUp(object sender, Mouse.MouseButtonEventArgs args)
        {
            this.layouts.ForEach(l => l.InjectMouseUpEvent(args.Position.x, args.Position.y, args.ChangedButton.ToGuiButton()));
        }

        private void AddDebugLayout()
        {
            this.debugLayout = new Layout("Debug", this.framework.AssetManager);
            var fpsText = new TextBlock(debugLayout, this.framework.AssetManager)
            {
                Font = framework.AssetManager.DefaultFont,
                Name = fpsTextName,
                Foreground = Color.White
            };
            AddLayout(this.debugLayout);
        }
        #endregion
    }

    internal static class GUIMapping
    {
        public static Irelia.Gui.MouseButton ToGuiButton(this Input.MouseButton inputButton)
        {
            switch (inputButton)
            {
                case Input.MouseButton.Left:
                    return Irelia.Gui.MouseButton.Left;
                case Input.MouseButton.Middle:
                    return Irelia.Gui.MouseButton.Middle;
                case Input.MouseButton.Right:
                    return Irelia.Gui.MouseButton.Right;
                default:
                    Debug.Assert(false, "Invalid input button, " + inputButton);
                    return 0;
            }
        }
    }
}
