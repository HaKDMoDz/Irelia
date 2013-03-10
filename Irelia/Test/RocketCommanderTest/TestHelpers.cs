using System;
using Input;
using Irelia.Gui;
using Irelia.Render;
using RocketCommander;

namespace RocketCommanderTest
{
    internal class TestGame : RocketCommanderGame
    {
        public event EventHandler<EventArgs> Initialized = delegate { };
        public event EventHandler<EventArgs> Updated = delegate { };

        public Keyboard Keyboard { get { return this.keyboard; } }
        public Mouse Mouse { get { return this.mouse; } }
        public Framework Framework { get { return this.framework; } }
        public SceneManager SceneManager { get { return this.sceneManager; } }
        public string DebugText
        {
            get
            {
                var fpsText = this.debugLayout.GetElement(this.fpsTextName) as TextBlock;
                return fpsText.Text;
            }
            set
            {
                var fpsText = this.debugLayout.GetElement(this.fpsTextName) as TextBlock;
                fpsText.Text = value;
            }
        }

        public TestGame()
            : base(1024, 768)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            SetScreen(null);
            Camera = new FreeCamera(Mouse, Keyboard);
            DisableRoamingRocketScene();

            Initialized(this, EventArgs.Empty);
        }

        protected override void Update(int elapsed)
        {
            base.Update(elapsed);

            DebugText += "\n\nCamera Type      : " + Camera.GetType().Name;
            DebugText += "\nCamera EyePos    : " + Camera.EyePos;
            DebugText += "\nCamera Direction : " + Camera.Direction;

            Updated(this, EventArgs.Empty);
        }
    }
}
