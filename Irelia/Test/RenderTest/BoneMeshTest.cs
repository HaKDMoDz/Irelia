//using System.IO;
//using Irelia.Render;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace RenderTest
//{
//    [TestClass()]
//    public class BoneMeshTest
//    {
//        private Device Device { get; set; }
//        private AssetManager ResourceManager { get; set; }

//        [TestInitialize()]
//        public void SetUp()
//        {
//            Device = TestHelpers.CreateDevice();
//            ResourceManager = new AssetManager(Device, RenderSettings.MediaPath);
//        }

//        private BoneMesh CreateVikingBoneMesh()
//        {
//            return new BoneMesh(Device, ResourceManager, Path.Combine(RenderSettings.MediaPath, "Test/viking3.x"), "viking");
//        }

//        [TestMethod()]
//        public void BoneMesh_Constructor_Test()
//        {
//            var boneMesh = CreateVikingBoneMesh();
//            Assert.IsTrue(boneMesh.FaceCount > 0);
//            Assert.IsTrue(boneMesh.VertexCount > 0);
//            Assert.IsTrue(boneMesh.BoneNames.Count > 0);
//            foreach (string boneName in boneMesh.BoneNames)
//            {
//                Assert.IsFalse(string.IsNullOrWhiteSpace(boneName));
//            }
//            Assert.IsTrue(boneMesh.AllBoneNames.Count >= boneMesh.BoneNames.Count);
//        }
        
//        [TestMethod()]
//        public void BoneMesh_Render_Test()
//        {
//            var boneMesh = CreateVikingBoneMesh();

//            Device.RawDevice.BeginScene();
//            Assert.IsTrue((boneMesh as IRenderable).Render(new Camera(), new Light()));
//            Device.RawDevice.EndScene();
//        }

//        [TestMethod()]
//        public void BoneMesh_Animate_Test()
//        {
//            var boneMesh = CreateVikingBoneMesh();

//            Assert.IsTrue(boneMesh.Animate(0.0f));
//            Assert.IsTrue(boneMesh.Animate(1.0f));
//        }

//        [TestMethod()]
//        public void BoneMesh_AnimationSet_Test()
//        {
//            var boneMesh = CreateVikingBoneMesh();
//            Assert.AreEqual(0, boneMesh.AnimationSet);
//            Assert.IsTrue(boneMesh.AnimationSetCount == 1);

//            boneMesh.AnimationSet = boneMesh.AnimationSetCount - 1;
//            Assert.AreEqual(boneMesh.AnimationSetCount - 1, boneMesh.AnimationSet);

//            Assert.AreEqual("CombinedAnim", boneMesh.GetAnimationSetName(0));
//        }
//    }
//}
