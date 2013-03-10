using Input;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class RoamingCamera : GameCamera
    {
        private readonly Game game;

        public RoamingCamera(Mouse mouse, Keyboard keyboard, Game game)
            : base(mouse, keyboard)
        {
            this.game = game;
        }

        public override void Activate()
        {
        }

        public override void Deactivate()
        {
        }

        public override void Update(int elapsed)
        {
            float rotateFactor = elapsed * 0.125f / 1000.0f;
            var yawAmount = new Radian((0.4f + 0.25f * MathUtils.Sin(new Radian(this.game.TotalGameTime / 15040))) * rotateFactor);
            Yaw(yawAmount);
            var pitchAmount = new Radian((0.35f + 0.212f * MathUtils.Cos(new Radian(this.game.TotalGameTime / 38040))) * rotateFactor);

            float moveFactor = elapsed * 27.5f / 1000.0f;
            MoveRelative(Vector3.UnitZ * moveFactor);
        }
    }
}
