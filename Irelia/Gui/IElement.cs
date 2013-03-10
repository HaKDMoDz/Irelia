using System;
using System.Xml;
using Irelia.Render;

namespace Irelia.Gui
{
    public interface IElement
    {
        Rectangle AbsRect { get; }

        event EventHandler PositionChanged;
        event EventHandler SizeChanged;
    }
}
