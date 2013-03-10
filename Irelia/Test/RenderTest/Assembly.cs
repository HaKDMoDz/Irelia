using System.IO;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public sealed class Assembly
    {
        [AssemblyInitialize()]
        public static void OnAssemblyInitialized(TestContext context)
        {
            string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Media");
            RenderSettings.MediaPath = Path.GetFullPath(mediaPath);
        }

        [TestMethod()]
        public void DummyTest()
        {
            Assert.AreNotEqual(true, false);
        }
    }
}
