using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RenderTest
{
    [TestClass()]
    public class CubeTextureTest
    {
        [TestMethod()]
        public void CubeTexture_Constructor_Test()
        {
            var cubeTex = new CubeTexture(TestHelpers.GetDevice(), TestHelpers.GetAssetFullPath("SpaceSkyCubeMap.dds"), "Cube");
            Assert.IsNotNull(cubeTex);
            Assert.AreEqual("Cube", cubeTex.Name);
        }
    }
}
