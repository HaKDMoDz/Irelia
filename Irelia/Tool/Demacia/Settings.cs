using System;
using Irelia.Render;

namespace Demacia
{
    public class Settings
    {
        public static Uri Theme
        {
            get { return Properties.Settings.Default.Theme; }
            set { Properties.Settings.Default.Theme = value; }
        }

        public static string BitmapFontBuilderPath
        {
            get { return Properties.Settings.Default.BitmapFontBuilderPath; }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
