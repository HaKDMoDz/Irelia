using Irelia.Render;
using Input;

namespace RocketCommander
{
    public abstract class GameCamera : Camera
    {
        protected readonly Mouse mouse;
        protected readonly Keyboard keyboard;

        public GameCamera(Mouse mouse, Keyboard keyboard)
        {
            this.mouse = mouse;
            this.keyboard = keyboard;
        }

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Update(int elapsed);
    }
}
