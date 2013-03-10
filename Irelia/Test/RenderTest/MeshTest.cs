using System.Collections.Generic;
using System.Linq;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class MeshTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        private List<string> MeshExtensions
        {
            get { return new MeshFactory().FileExtensions.ToList(); }
        }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        private Mesh GetMesh()
        {
            return AssetManager.Load(@"Test\tiger.x") as Mesh;
        }

        [TestMethod()]
        public void Mesh_StockMesh_Test()
        {
            var sphere = Mesh.CreateSphere(Device, AssetManager, AssetManager.DefaultMaterial, 5.0f, 80, 80);
            Assert.IsNotNull(sphere);
        }

        [TestMethod()]
        public void Mesh_CreateFromXFile_Test()
        {
            var mesh = Mesh.CreateFromXFile(Device, AssetManager, TestHelpers.SampleDirectXMeshPath, "SampleMesh");
            Assert.IsNotNull(mesh);
        }

        [TestMethod()]
        public void Mesh_GetBoudingRadius_Test()
        {
            var mesh = GetMesh();
            Assert.IsTrue(mesh.BoundingCenter != Vector3.Zero);
            Assert.IsTrue(mesh.BoundingRadius > 0.0f);
        }

        [TestMethod()]
        public void Mesh_LoadSave_Test()
        {
            var mesh1 = GetMesh();

            MeshExtensions.ForEach(ext =>
            {
                string fileName = "tiger" + ext;
                mesh1.Save(fileName);

                var mesh2 = Mesh.Load(Device, fileName, AssetManager, "tiger");
                Assert.AreEqual(mesh1.BoundingCenter, mesh2.BoundingCenter);
                Assert.AreEqual(mesh1.BoundingRadius, mesh2.BoundingRadius);
            });
        }

        [TestMethod()]
        public void Mesh_FlipTextureV_Test()
        {
            var mesh = GetMesh();
            mesh.FlipTextureV();
        }

        [TestMethod()]
        public void Mesh_GetVertices_Test()
        {
            var mesh = GetMesh();
            Assert.IsTrue(mesh.Vertices.Count > 0);
        }

        [TestMethod()]
        public void MeshFactory_Test()
        {
            MeshExtensions.ForEach(ext =>
            {
                Assert.AreEqual(typeof(Mesh), AssetManager.GetAssetType("sample" + ext));

                string meshFile = @"Test\tiger" + ext;
                GetMesh().Save(AssetManager.GetFullPath(meshFile));

                var mesh = AssetManager.Load(meshFile) as Mesh;
                Assert.AreEqual(meshFile, mesh.Name);
            });
        }
    }
}
