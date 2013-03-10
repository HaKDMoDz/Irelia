using Irelia.Gui;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class MissionSelectionScreen : Screen
    {
        private Layout layout;

        public MissionSelectionScreen(RocketCommanderGame game, Framework framework)
            : base(game, framework)
        {
        }

        public override void Enter()
        {
            this.layout = this.framework.AssetManager.Load(@"RocketCommander\GUI\MissionSelection.layout") as Layout;
            this.game.AddLayout(this.layout);

            GetButton("BackButton").MouseLeftButtonDown += backButton_MouseLeftButtonDown;
        }
        
        public override void Update()
        {
        }

        public override void Exit()
        {
            GetButton("BackButton").MouseLeftButtonDown -= backButton_MouseLeftButtonDown;

            this.game.RemoveLayout(this.layout);
        }

        private void backButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.game.SetScreen(new MainMenuScreen(this.game, this.framework));
        }

        private Button GetButton(string name)
        {
            return this.layout.GetElement(name) as Button;
        }
    }
}
