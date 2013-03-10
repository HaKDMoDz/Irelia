using Input;
using Irelia.Render;

namespace RocketCommander
{
    public sealed class FreeCamera : GameCamera
    {
        private Vector2 lastMousePos;

        public FreeCamera(Mouse mouse, Keyboard keyboard)
            : base(mouse, keyboard)
        {
            this.lastMousePos = mouse.Position;
        }

        public override void Activate()
        {
            this.mouse.MouseMove += OnMouseMove;
        }

        public override void Deactivate()
        {
            this.mouse.MouseMove -= OnMouseMove;
        }

        public override void Update(int elapsed)
        {
            float moveSensitivity = 5.0f;
            float rotateSensitivity = 0.1f;

            if (this.keyboard.IsKeyDown(Key.W))
            {
                MoveRelative(Vector3.UnitZ * moveSensitivity);
            }
            else if (this.keyboard.IsKeyDown(Key.S))
            {
                MoveRelative(-Vector3.UnitZ * moveSensitivity);
            }
            else if (this.keyboard.IsKeyDown(Key.A))
            {
                MoveRelative(-Vector3.UnitX * moveSensitivity);
            }
            else if (this.keyboard.IsKeyDown(Key.D))
            {
                MoveRelative(Vector3.UnitX * moveSensitivity);
            }
            else if (this.keyboard.IsKeyDown(Key.F))
            {
                Yaw(new Radian(-rotateSensitivity));
            }
            else if (this.keyboard.IsKeyDown(Key.G))
            {
                Yaw(new Radian(rotateSensitivity));
            }
            else if (this.keyboard.IsKeyDown(Key.R))
            {
                Pitch(new Radian(-rotateSensitivity));
            }
            else if (this.keyboard.IsKeyDown(Key.T))
            {
                Pitch(new Radian(rotateSensitivity));
            }
        }

        private void OnMouseMove(object sender, Mouse.MouseEventArgs args)
        {
            var delta = args.Position - this.lastMousePos;

            if (args.RightButton == MouseButtonState.Pressed)
            {
                float sensitivity = 0.002f;

                Yaw(new Radian(-delta.x * sensitivity));
                Pitch(new Radian(-delta.y * sensitivity));
            }
            else if (args.MiddleButton == MouseButtonState.Pressed)
            {
                float sensitivity = 0.5f;
                var moveAmount = new Vector3(delta.x * sensitivity, -delta.y * sensitivity, 0.0f);
                MoveRelative(moveAmount);
            }

            this.lastMousePos = args.Position;
        }
    }
}
