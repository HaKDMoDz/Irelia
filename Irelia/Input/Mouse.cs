using System;
using SlimDX.DirectInput;
using DIMouse = SlimDX.DirectInput.Mouse;
using Irelia.Render;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Input
{
    public enum MouseButtonState
    {
        Released,
        Pressed
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public sealed class Mouse
    {
        public enum DIButton
        {
            LeftButton = 0,
            RightButton = 1,
            MiddleButton = 2
        }

        #region Eventes
        public class MouseEventArgs : EventArgs
        {
            public MouseButtonState LeftButton { get; private set; }
            public MouseButtonState RightButton { get; private set; }
            public MouseButtonState MiddleButton { get; private set; }
            public Vector2 Position { get; private set; }

            public MouseEventArgs(Mouse mouse)
            {
                LeftButton = mouse.LeftButton;
                RightButton = mouse.RightButton;
                MiddleButton = mouse.MiddleButton;
                Position = mouse.Position;
            }
        }

        public class MouseButtonEventArgs : MouseEventArgs
        {
            public MouseButtonState ButtonState { get; private set; }
            public MouseButton ChangedButton { get; private set; }

            public MouseButtonEventArgs(Mouse mouse, MouseButtonState buttonState, MouseButton changedButton)
                : base(mouse)
            {
                ButtonState = buttonState;
                ChangedButton = changedButton;
            }
        }

        public EventHandler<MouseEventArgs> MouseMove = delegate { };
        public EventHandler<MouseButtonEventArgs> MouseDown = delegate { };
        public EventHandler<MouseButtonEventArgs> MouseUp = delegate { };
        #endregion

        #region P/Invoke
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        #endregion

        #region Fields
        private IntPtr windowHandle;
        private readonly DIMouse mouse;
        private readonly bool exclusive = false;
        private Vector2 position;
        private MouseState lastState;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get
            {
                if (this.exclusive)
                {
                    var state = GetMouseState();
                    if (state != null)
                        this.position += new Vector2(state.X, state.Y);
                }
                else
                {
                    POINT point;
                    if (GetCursorPos(out point) == false)
                        return this.position;
                    if (ScreenToClient(this.windowHandle, ref point) == false)
                        return this.position;
                    this.position = new Vector2(point.X, point.Y);
                }

                return this.position;
            }
        }

        public MouseButtonState LeftButton
        {
            get
            {
                return GetMouseButtonState(GetMouseState(), DIButton.LeftButton);
            }
        }

        public MouseButtonState RightButton
        {
            get
            {
                return GetMouseButtonState(GetMouseState(), DIButton.RightButton);
            }
        }

        public MouseButtonState MiddleButton
        {
            get
            {
                return GetMouseButtonState(GetMouseState(), DIButton.MiddleButton);
            }
        }
        #endregion

        #region Constructors
        public Mouse(IntPtr windowHandle)
        {
            this.windowHandle = windowHandle;

            var dinput = new DirectInput();

            this.mouse = new DIMouse(dinput);
            var cooperativeLevel = CooperativeLevel.Foreground;
            cooperativeLevel |= exclusive? CooperativeLevel.Exclusive : CooperativeLevel.Nonexclusive;
            this.mouse.SetCooperativeLevel(windowHandle, cooperativeLevel);
            this.mouse.Acquire();
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            var state = GetMouseState();
            if (state == null)
                return;

            if (this.lastState == null)
            {
                this.lastState = state;
                return;
            }
            
            if (state.X != this.lastState.X || state.Y != this.lastState.Z)
            {
                MouseMove(this, new MouseEventArgs(this));
            }

            var button = DIButton.LeftButton;
            var lastButtonState = GetMouseButtonState(this.lastState, button);
            var buttonState = GetMouseButtonState(state, button);
            if (lastButtonState != buttonState)
            {
                if (buttonState == MouseButtonState.Pressed)
                    MouseDown(this, new MouseButtonEventArgs(this, buttonState, button.ToMouseButton()));
                else if (buttonState == MouseButtonState.Released)
                    MouseUp(this, new MouseButtonEventArgs(this, buttonState, button.ToMouseButton()));
            }

            this.lastState = state;
        }
        #endregion

        #region Private Methods

        private MouseState GetMouseState()
        {
            if (this.mouse.Acquire().IsFailure)
                return null;

            if (this.mouse.Poll().IsFailure)
                return null;

            var state = new MouseState();
            if (this.mouse.GetCurrentState(ref state).IsFailure)
                return null;

            return state;
        }

        private MouseButtonState GetMouseButtonState(MouseState mouseState, DIButton button)
        {
            if (mouseState == null)
                return MouseButtonState.Released;

            return mouseState.IsPressed((int)button) ? MouseButtonState.Pressed : MouseButtonState.Released;
        }
        #endregion
    }
}
