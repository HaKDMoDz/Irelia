using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class MeshVertexTest
    {
        [TestMethod()]
        public void MeshVertex_Clone_Test()
        {
            var mv = new MeshVertex()
            {
                Normal = Vector3.UnitX,
                Position = Vector3.UnitY,
                UV = Vector2.Zero,
                Tangent = Vector3.UnitZ,
                Binormal = Vector3.Zero,
                Color = Color.White
            };

            Assert.AreEqual(mv, mv.Clone());
        }
    }
}
