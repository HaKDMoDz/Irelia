using System.IO;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaciaTest
{
    [TestClass()]
    public class Assembly
    {
        [AssemblyInitialize()]
        public static void OnAssemblyInitialized(TestContext context)
        {
            string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Media");
            RenderSettings.MediaPath = Path.GetFullPath(mediaPath);
        }
    }
}
