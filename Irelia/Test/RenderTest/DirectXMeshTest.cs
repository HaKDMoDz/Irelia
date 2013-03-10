using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class DirectXMeshTest
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
        public void DirectXMeshFactory_Test()
        {
            Assert.AreEqual(typeof(DirectXMesh), AssetManager.GetAssetType("sample.x"));

            // Normal mesh
            string path = @"Test\tiger.x";
            Mesh mesh = AssetManager.Load(path) as Mesh;
            Assert.IsNotNull(mesh);
            Assert.AreEqual(path, mesh.Name);

            // Reload with changing option
            var args = new AssetLoadArguments();
            args.Add("loadHierarchy", true);
            Assert.AreNotEqual(mesh, AssetManager.Load(TestHelpers.SampleDirectXMeshPath, args) as Mesh);

            //// Bone-mesh
            //args = new Arguments();
            //args.Add("loadHierarchy", true);
            //path = @"Test\viking3.x";
            //BoneMesh boneMesh = resMgr.Load(path, args) as BoneMesh;
            //Assert.IsNotNull(boneMesh);
            //Assert.AreEqual(path, boneMesh.Name);
        }
    }
}
