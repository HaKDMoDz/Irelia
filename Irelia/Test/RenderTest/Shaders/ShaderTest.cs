using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RenderTest
{
    [TestClass()]
    public class ShaderTest
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
        public void Shader_Constructor_Test()
        {
            var shader = new Shader(Device, TestHelpers.SampleShaderPath, "SimpleShader");
            Assert.AreEqual("SimpleShader", shader.Name);
            Assert.AreEqual(shader.Name, shader.ToString());
        }

        [TestMethod()]
        public void Shader_Render_With_Material_Test()
        {
            var shader = TestHelpers.CreateSimpleShader(Device);

            Assert.IsFalse(shader.Render(new Material("Material"), new Light(), new Camera(), Matrix4.Identity, (p) => false));
            Assert.IsTrue(shader.Render(new Material("Material"), new Light(), new Camera(), Matrix4.Identity, (p) => true));
        }

        [TestMethod()]
        public void Shader_RenderWithNonExistingTechnique_Test()
        {
            var shader = TestHelpers.CreateSimpleShader(Device);
            var material = new Material("Material")
            {
                Technique = "NonExistingTechnique"
            };

            Assert.IsFalse(shader.Render(material, new Light(), new Camera(), Matrix4.Identity, (p) => true));
        }

        [TestMethod()]
        public void Shader_UberShader_Test()
        {
            var shader = new Shader(Device, Path.Combine(RenderSettings.MediaPath, "Engine/UberShader.fx"), "Engine/UberShader.fx");
            Assert.IsTrue(shader.Render(new Material("Material"), new Light(), new Camera(), Matrix4.Identity, (p) => true));
        }

        [TestMethod()]
        public void Shader_Reload_Test()
        {
            var shader = TestHelpers.CreateSimpleShader(Device);
            Assert.IsTrue(shader.Render(new Material("Material"), new Light(), new Camera(), Matrix4.Identity, (p) => true));

            Assert.IsTrue(shader.Reload());
            Assert.IsTrue(shader.Render(new Material("Material"), new Light(), new Camera(), Matrix4.Identity, (p) => true));
        }

        [TestMethod()]
        public void ShaderFactory_Test()
        {
            Assert.AreEqual(typeof(Shader), AssetManager.GetAssetType("sample.fx"));

            var files = new string[] { @"Engine\FlatTexture.fx", @"Engine\Skybox.fx", @"Engine\UberShader.fx" };
            foreach (var file in files)
            {
                var shader = AssetManager.Load(file) as Shader;
                Assert.IsNotNull(shader);
                Assert.AreEqual(file, shader.Name);
            }
        }
    }
}
