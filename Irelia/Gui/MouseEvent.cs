using System;
using Irelia.Render;

namespace Irelia.Gui
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public class MouseEventArgs : EventArgs
    {
        public Vector2 Position
        {
            get;
            private set;
        }

        public bool Handled;

        public MouseEventArgs(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public Vector2 GetPosition(IElement relativeTo)
        {
            return Position - relativeTo.AbsRect.Location;
        }
    }

    public sealed class MouseButtonEventArgs : MouseEventArgs
    {
        public MouseButtonEventArgs(float x, float y, MouseButton changedButton)
            : base(x, y)
        {
            ChangedButton = changedButton;
        }

        public MouseButton ChangedButton { get; private set; }        
    }
}
