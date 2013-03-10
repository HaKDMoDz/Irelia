using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RenderTest
{
    [TestClass()]
    public class TerrainTest
    {
        private Device Device { get; set; }
        private AssetManager ResourceManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            ResourceManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        private Terrain CreateTerrain(int width, int length, int maxHeight)
        {
            return new Terrain(Device, ResourceManager, width, length, maxHeight, new string[] {"Test/water.bmp", "Test/slime2.bmp", "Test/slime1.bmp"});
        }

        [TestMethod()]
        public void Terrain_Render_Test()
        {
            int width = 16, length = 16, maxHeight = 10;
            var terrain = CreateTerrain(width, length, maxHeight);
            
            Assert.AreEqual(width, terrain.Width);
            Assert.AreEqual(length, terrain.Length);
            Assert.AreEqual(maxHeight, terrain.MaxHeight);

            Device.RawDevice.BeginScene();
            Assert.IsTrue((terrain as IRenderable).Render(new Camera(), new Light()));
            Device.RawDevice.EndScene();
        }

        [TestMethod()]
        public void Terrain_GetHeight_Test()
        {
            int width = 16, length = 16, maxHeight = 10;
            var terrain = CreateTerrain(width, length, maxHeight);

            // No exceptions
            Assert.IsTrue(Math.Abs(terrain.GetHeight(0, 0)) <= maxHeight);
            Assert.IsTrue(Math.Abs(terrain.GetHeight(width - 1, 0)) <= maxHeight);
            Assert.IsTrue(Math.Abs(terrain.GetHeight(0, length - 1)) <= maxHeight);
            Assert.IsTrue(Math.Abs(terrain.GetHeight(width - 1, length - 1)) <= maxHeight);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Terrain_GetHeight_Fail_Test()
        {
            int width = 16, length = 16, maxHeight = 10;
            var terrain = CreateTerrain(width, length, maxHeight);

            terrain.GetHeight(width, length);
        }
    }
}
