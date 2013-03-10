using System.Collections.Generic;
using System.Linq;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class MaterialTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        private List<string> MaterialExtensions
        {
            get { return new MaterialFactory().FileExtensions.ToList(); }
        }

        private Material CreateMaterial(string name)
        {
            return new Material(name)
            {
                Shader = AssetManager.Load(@"Engine\UberShader.fx") as Shader
            };
        }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        [TestMethod()]
        public void Material_DefaultValues_Test()
        {
            var mat = new Material("Material");
            Assert.AreEqual("Material", mat.Name);
            Assert.AreEqual("SimpleEffect", mat.Technique);
        }

        [TestMethod()]
        public void Material_SaveLoad_Test()
        {
            MaterialExtensions.ForEach(ext =>
            {
                string name = "Material";
                string fileName = name + ext;

                var mat1 = new Material(name)
                {
                    Shader = AssetManager.Load(@"Engine\UberShader.fx") as Shader,
                    DiffuseTexture = AssetManager.Load(TestHelpers.SampleTexturePath) as Texture,
                    UseNormalMap = true,
                    NormalTexture = AssetManager.Load(TestHelpers.SampleTexturePath) as Texture,
                    AmbientColor = Color.Black,
                    DiffuseColor = Color.Blue,
                    EmissiveColor = Color.Green,
                    SpecularPower = 0.03f,
                    SpecularColor = Color.Red,
                    Technique = "SimpleEffect",
                    UseOffsetMapping = true,
                    HeightMapTexture = AssetManager.Load(TestHelpers.SampleTexturePath) as Texture,
                    OffsetHeight = 0.01f,
                };
                mat1.Save(fileName);

                var mat2 = Material.Load(Device, fileName, name, AssetManager);
                Assert.AreEqual(name, mat2.Name);
                Assert.AreEqual(mat1.Shader, mat2.Shader);
                Assert.AreEqual(mat1.DiffuseTexture, mat2.DiffuseTexture);
                Assert.AreEqual(mat1.UseNormalMap, mat2.UseNormalMap);
                Assert.AreEqual(mat1.NormalTexture, mat2.NormalTexture);
                Assert.AreEqual(mat1.AmbientColor, mat2.AmbientColor);
                Assert.AreEqual(mat1.DiffuseColor, mat2.DiffuseColor);
                Assert.AreEqual(mat1.SpecularPower, mat2.SpecularPower);
                Assert.AreEqual(mat1.SpecularColor, mat2.SpecularColor);
                Assert.AreEqual(mat1.Technique, mat2.Technique);
                Assert.AreEqual(mat1.UseOffsetMapping, mat2.UseOffsetMapping);
                Assert.AreEqual(mat1.HeightMapTexture, mat2.HeightMapTexture);
                Assert.AreEqual(mat1.OffsetHeight, mat2.OffsetHeight);
            });
        }

        [TestMethod()]
        public void MaterialFactory_Test()
        {
            MaterialExtensions.ForEach(ext =>
            {
                Assert.AreEqual(typeof(Material), AssetManager.GetAssetType("sample" + ext));

                string matFile = @"Test\tiger" + ext;
                CreateMaterial("tiger").Save(AssetManager.GetFullPath(matFile));

                var material = AssetManager.Load(matFile) as Material;
                Assert.AreEqual(matFile, material.Name);
            });
        }
    }
}
