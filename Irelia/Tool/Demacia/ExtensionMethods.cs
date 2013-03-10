using Irelia.Render;
using WM = System.Windows.Media;

namespace Demacia
{
    public static class ExtensionMethods
    {
        public static Color ToIreliaColor(this WM.Color color)
        {
            return new Color()
            {
                a = color.A / 255.0f,
                r = color.R / 255.0f,
                g = color.G / 255.0f,
                b = color.B / 255.0f
            };
        }

        public static WM.Color ToWindowsColor(this Color color)
        {
            return new WM.Color()
            {
                A = (byte)(color.a * 255.0f),
                R = (byte)(color.r * 255.0f),
                G = (byte)(color.g * 255.0f),
                B = (byte)(color.b * 255.0f)
            };
        }
    }
}
