using System.IO;
using System.Diagnostics;
using System;

namespace Irelia.Render
{
    // Class for specifying settings information to Render project
    public class RenderSettings
    {
        public static string MediaPath
        {
            set { Properties.Settings.Default.MediaPath = value; }
            get 
            {
                var path = Properties.Settings.Default.MediaPath;
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new InvalidOperationException("Media path not set");
                }
                return path; 
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
