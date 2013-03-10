using System.Collections.Generic;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class TangentSpaceCalculatorTest
    {
        [TestMethod()]
        public void TangentSpaceCalculatorConstructorTest()
        {
            var vertices = new List<MeshVertex>()
            {
                new MeshVertex() { Position = Vector3.Zero, Normal = Vector3.UnitY, UV = Vector2.UnitY },
                new MeshVertex() { Position = Vector3.UnitZ, Normal = Vector3.UnitY, UV = Vector2.Zero },
                new MeshVertex() { Position = Vector3.UnitX, Normal = Vector3.UnitY, UV = Vector2.UnitX }
            };
            var indices = new List<short>() {0, 1, 2};

            var calc = new TangentSpaceCalculator();
            var actual = calc.Build(vertices, indices);
            Assert.AreEqual(vertices.Count, actual.Count);
        }
    }
}
