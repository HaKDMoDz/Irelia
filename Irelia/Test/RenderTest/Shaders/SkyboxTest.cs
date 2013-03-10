using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RenderTest
{
    [TestClass()]
    public class SkyboxTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void Skybox_Render_Test()
        {
            var sky = new Skybox(Device, AssetManager)
            {
                SkyTexture = new CubeTexture(Device, AssetManager.GetFullPath(@"Test/SpaceSkyCubeMap.dds"), ""),
                AmbientColor = Color.Red
            };

            Device.RawDevice.BeginScene();
            Assert.IsTrue((sky as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }
    }
}
