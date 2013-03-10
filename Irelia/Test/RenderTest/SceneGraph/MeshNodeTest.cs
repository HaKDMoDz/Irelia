using System.IO;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class MeshNodeTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        private MeshNode CreateMeshNode()
        {
            var mesh = Mesh.Load(Device, Path.Combine(RenderSettings.MediaPath, @"Test\tiger.meshs"), AssetManager, "Tiger");
            return new MeshNode(Device, mesh);
        }

        [TestMethod()]
        public void MeshNode_DefaultValues_Test()
        {
            var meshNode = CreateMeshNode();
            Assert.AreEqual(Quaternion.Identity, meshNode.Orientation);
            Assert.AreEqual(Vector3.Zero, meshNode.Position);
            Assert.AreEqual(new Vector3(1.0f, 1.0f, 1.0f), meshNode.Scale);
        }

        [TestMethod()]
        public void MeshNode_Render_Test()
        {
            var meshNode = CreateMeshNode();
            Assert.IsTrue((meshNode as IRenderable).Render(new Camera(), new Light()));
        }

        //[TestMethod()]
        //public void Mesh_Position_Test()
        //{
        //    var mesh = CreateMesh();
        //    Assert.AreEqual(Vector3.Zero, mesh.Position);

        //    var newPos = new Vector3(1.0f, 2.0f, 3.0f);
        //    mesh.Position = newPos;
        //    Assert.AreEqual(newPos, mesh.Position);
        //}

        //[TestMethod()]
        //public void Mesh_Orientation_Test()
        //{
        //    var mesh = CreateMesh();
        //    Assert.AreEqual(Quaternion.Identity, mesh.Orientation);

        //    var newOrient = new Quaternion(1.0f, 2.0f, 3.0f, 4.0f);
        //    mesh.Orientation = newOrient;
        //    Assert.AreEqual(newOrient, mesh.Orientation);
        //}

        //[TestMethod()]
        //public void Mesh_Scale_Test()
        //{
        //    var mesh = CreateMesh();
        //    Assert.AreEqual(new Vector3(1.0f, 1.0f, 1.0f), mesh.Scale);

        //    var newScale = new Vector3(1.0f, 2.0f, 3.0f);
        //    mesh.Scale = newScale;
        //    Assert.AreEqual(newScale, mesh.Scale);
        //}

        //[TestMethod()]
        //public void Mesh_WorldMatrix_Test()
        //{
        //    var pos = new Vector3(1.1f, 2.2f, 3.333f);
        //    var orient = Quaternion.CreateFromAxisAngle(Vector3.UnitX, new Radian(MathUtils.PI / 2.0f));
        //    var scale = new Vector3(1.0f, 2.0f, 3.0f);

        //    var mesh = CreateMesh();
        //    mesh.Position = pos;
        //    mesh.Orientation = orient;
        //    mesh.Scale = scale;

        //    // SRT
        //    var expected = Matrix4.CreateScale(scale) *
        //        Matrix4.CreateFromQuaternion(orient) *
        //        Matrix4.CreateTranslation(pos);
        //    Assert.AreEqual(expected, mesh.WorldMatrix);
        //}
    }
}
