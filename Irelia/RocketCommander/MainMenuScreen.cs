using Irelia.Gui;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class MainMenuScreen : Screen
    {
        private Layout layout;

        public MainMenuScreen(RocketCommanderGame game, Framework framework)
            : base(game, framework)
        {
        }

        public override void Enter()
        {
            this.layout = this.framework.AssetManager.Load(@"RocketCommander\GUI\MainMenu.layout") as Layout;
            this.game.AddLayout(this.layout);

            GetButton("MissionsButton").MouseLeftButtonDown += missionsButton_MouseLeftButtonDown;
            GetButton("HelpButton").MouseLeftButtonDown += helpButton_MouseLeftButtonDown;
            GetButton("ExitButton").MouseLeftButtonDown += exitButton_MouseLeftButtonDown;

            game.EnableRoamingRocketScene();
        }        

        public override void Update()
        {
        }

        public override void Exit()
        {
            GetButton("MissionsButton").MouseLeftButtonDown -= missionsButton_MouseLeftButtonDown;
            GetButton("HelpButton").MouseLeftButtonDown -= helpButton_MouseLeftButtonDown;
            GetButton("ExitButton").MouseLeftButtonDown -= exitButton_MouseLeftButtonDown;

            this.game.RemoveLayout(this.layout);
        }

        private void missionsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.game.SetScreen(new MissionSelectionScreen(this.game, this.framework));
        }

        private void helpButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.game.SetScreen(new HelpScreen(this.game, this.framework));
        }

        private void exitButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.game.Exit();
        }

        private Button GetButton(string name)
        {
            return this.layout.GetElement(name) as Button;
        }
    }
}
