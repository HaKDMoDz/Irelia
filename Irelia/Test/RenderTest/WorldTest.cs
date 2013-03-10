using System.Collections.Generic;
using System.Linq;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class WorldTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        private List<string> WorldExtensions
        {
            get { return new WorldFactory().FileExtensions.ToList(); }
        }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void World_DefaultValues_Test()
        {
            var world = new World(Device, "World", AssetManager);
            Assert.AreEqual("World", world.Name);
        }

        [TestMethod()]
        public void World_SaveLoad_Test()
        {
            WorldExtensions.ForEach(ext =>
            {
                string name = "World";
                string fileName = name + ext;

                AssetLoadArguments args = new AssetLoadArguments();
                args.Add("cube", true);

                var world1 = new World(Device, name, AssetManager);
                world1.Skybox.SkyTexture = AssetManager.Load(@"Test\SpaceSkyCubeMap.dds", args) as CubeTexture;
                world1.DarkenBorder.IsEnabled = false;
                world1.DarkenBorder.BorderTexture = AssetManager.Load(@"Engine\ScreenBorderFadeout.dds") as Texture;
                world1.Glow.IsEnabled = false;
                world1.Glow.RadialBlurScaleFactor = 1.0f;
                world1.Glow.BlurWidth = 7.0f;
                world1.Glow.GlowIntensity = 0.6f;
                world1.Glow.HighlightIntensity = 0.3f;
                world1.Save(fileName);

                var world2 = World.Load(Device, fileName, name, AssetManager);
                Assert.AreEqual(name, world2.Name);
                Assert.AreEqual(world1.Skybox.SkyTexture, world2.Skybox.SkyTexture);
                Assert.AreEqual(world1.DarkenBorder.IsEnabled, world2.DarkenBorder.IsEnabled);
                Assert.AreEqual(world1.DarkenBorder.BorderTexture, world2.DarkenBorder.BorderTexture);
                Assert.AreEqual(world1.Glow.IsEnabled, world2.Glow.IsEnabled);
                Assert.AreEqual(world1.Glow.RadialBlurScaleFactor, world2.Glow.RadialBlurScaleFactor);
                Assert.AreEqual(world1.Glow.BlurWidth, world2.Glow.BlurWidth);
                Assert.AreEqual(world1.Glow.GlowIntensity, world2.Glow.GlowIntensity);
                Assert.AreEqual(world1.Glow.HighlightIntensity, world2.Glow.HighlightIntensity);
            });
        }

        [TestMethod()]
        public void WorldFactory_Test()
        {
            WorldExtensions.ForEach(ext =>
            {
                Assert.AreEqual(typeof(World), AssetManager.GetAssetType("sample" + ext));

                string worldFile = @"Test\World" + ext;
                new World(Device, "world", AssetManager).Save(AssetManager.GetFullPath(worldFile));

                var world = AssetManager.Load(worldFile) as World;
                Assert.AreEqual(worldFile, world.Name);
            });

            
        }
    }
}
