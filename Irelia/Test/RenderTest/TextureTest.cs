using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RenderTest
{
    [TestClass()]
    public class TextureTest
    {
        private Device Device { get; set; }
        private AssetManager AssetManager { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
            AssetManager = new AssetManager(Device, RenderSettings.MediaPath);
        }

        private Texture CreateTexture(string name = "Texture")
        {
            return new Texture(Device, TestHelpers.SampleTexturePath, name);
        }

        [TestMethod()]
        public void Texture_Constructor_Test()
        {
            var texture = CreateTexture("TextureName");
            Assert.IsNotNull(texture);

            Assert.AreEqual("TextureName", texture.Name);
            Assert.AreEqual(1024, texture.Width);
            Assert.AreEqual(512, texture.Height);
        }

        [TestMethod()]
        public void Texture_FromToStream_Test()
        {
            var expected = CreateTexture("TextureName");
            var stream = new MemoryStream();
            expected.ToStream().CopyTo(stream);
            stream.Position = 0;

            var actual = new Texture(Device, stream, "TextureName");
            Assert.AreEqual(expected.Width, actual.Width);
        }

        [TestMethod()]
        public void Texture_GetRawTexture_Test()
        {
            var texture = CreateTexture();
            Assert.IsNotNull(texture.RawTexture);
        }

        [TestMethod()]
        public void Texture_ToString_Test()
        {
            var texture = CreateTexture();
            Assert.AreEqual(texture.Name, texture.ToString());
        }

        [TestMethod()]
        public void Texture_SaveToFile_Test()
        {
            var texture = CreateTexture();
            Assert.IsTrue(texture.SaveToFile("thumbnail.jpg"));
            Assert.IsTrue(File.Exists("thumbnail.jpg"));
        }

        [TestMethod()]
        public void TextureFactory_Test()
        {
            Assert.AreEqual(typeof(Texture), AssetManager.GetAssetType("sample.jpg"));
            Assert.AreEqual(typeof(Texture), AssetManager.GetAssetType("sample.bmp"));
            Assert.AreEqual(typeof(Texture), AssetManager.GetAssetType("sample.dds"));
            Assert.AreEqual(typeof(Texture), AssetManager.GetAssetType("sample.tga"));
            Assert.AreEqual(typeof(Texture), AssetManager.GetAssetType("sample.png"));

            string path = @"Test\SpaceSkyCubeMap.dds";
            var texture = AssetManager.Load(path) as Texture;
            Assert.IsNotNull(texture);
            Assert.AreEqual(path, texture.Name);

            // Load as cube texture
            var args = new AssetLoadArguments();
            args.Add("cube", true);
            var cubeTex = AssetManager.Load(path, args) as CubeTexture;
            Assert.IsNotNull(cubeTex);
            Assert.AreNotEqual(texture, cubeTex);
            Assert.AreEqual(path, cubeTex.Name);

            // Loading non-cube texture as cube texture fails
            Assert.IsNull(AssetManager.Load(@"Test\DefaultColor.dds"));
        }
    }
}
