using Irelia.Render;

namespace RocketCommander
{
    public abstract class Screen
    {
        protected readonly RocketCommanderGame game;
        protected readonly Framework framework;

        public Screen(RocketCommanderGame game, Framework framework)
        {
            this.game = game;
            this.framework = framework;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
