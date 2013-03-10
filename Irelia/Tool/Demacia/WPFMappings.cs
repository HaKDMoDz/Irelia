using Irelia.Gui;
using SysInput = System.Windows.Input;

namespace Demacia
{
    public static class WPFMappings
    {
        public static MouseButton ToIreliaButton(this SysInput.MouseButton sysBtn)
        {
            switch (sysBtn)
            {
                case SysInput.MouseButton.Right:
                    return MouseButton.Right;
                
                case SysInput.MouseButton.Middle:
                    return MouseButton.Middle;

                case SysInput.MouseButton.Left:
                default:
                    return MouseButton.Left;
            }
        }
    }
}
