using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class SceneManagerTest
    {
        private Device Device { get; set; }
        private RenderSystem RenderSystem { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            RenderSystem = new RenderSystem(Device, new SpriteRenderer(Device));
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void SceneManager_AddRemoveRenderable_Test()
        {
            var sceneMgr = new SceneManager(Device, AssetManager);
            var renderable = TestHelpers.CreateRenderableMock(Device);

            Assert.IsTrue(sceneMgr.AddRenderable(renderable));
            Assert.IsFalse(sceneMgr.AddRenderable(renderable));

            Assert.IsTrue(sceneMgr.RemoveRenderable(renderable));
            Assert.IsFalse(sceneMgr.RemoveRenderable(renderable));
        }

        [TestMethod()]
        public void SceneManager_AddRemoveSprite_Test()
        {
            var sceneMgr = new SceneManager(Device, AssetManager);
            var sprite = TestHelpers.CreateSpriteMock(Device);

            Assert.IsTrue(sceneMgr.AddSprite(sprite));
            Assert.IsFalse(sceneMgr.AddSprite(sprite));

            Assert.IsTrue(sceneMgr.RemoveSprite(sprite));
            Assert.IsFalse(sceneMgr.RemoveSprite(sprite));
        }

        [TestMethod()]
        public void SceneManager_Render_Test()
        {
            var sceneMgr = new SceneManager(Device, AssetManager);
            sceneMgr.AddRenderable(TestHelpers.CreateRenderableMock(Device));
            sceneMgr.AddSprite(TestHelpers.CreateSpriteMock(Device));
            Assert.IsTrue(sceneMgr.Render(RenderSystem, RenderSystem.PrimaryRenderTarget, false));
        }

        [TestMethod()]
        public void SceneManager_SetWorld_Test()
        {
            var sceneMgr = new SceneManager(Device, AssetManager);
            
            var world = new World(Device, "world", AssetManager);
            var args = new AssetLoadArguments();
            args.Add("cube", true);
            world.Skybox.SkyTexture = AssetManager.Load(@"Test\SpaceSkyCubeMap.dds", args) as CubeTexture;

            sceneMgr.World = world;
            Assert.AreEqual(world, sceneMgr.World);
        }
    }
}
