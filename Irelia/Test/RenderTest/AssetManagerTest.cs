using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace RenderTest
{
    [TestClass()]
    public class AssetManagerTest
    {
        private Device Device { get; set; }

        [TestInitialize()]
        public void SetUp()
        {
            Device = TestHelpers.GetDevice();
        }

        [TestMethod()]
        public void AssetManager_Arguments_GetValue_Test()
        {
            var args = new AssetLoadArguments();
            
            args.Add("intKey", 5);
            Assert.AreEqual(5, args.GetValue<int>("intKey", 0));
            
            args.Add("stringKey", "text");
            Assert.AreEqual("text", args.GetValue<string>("stringKey", ""));
        }

        [TestMethod()]
        public void AssetManager_Arguments_CaseInsensitive_Test()
        {
            var args = new AssetLoadArguments();
            args.Add("intKey", 5);
            Assert.AreEqual(5, args.GetValue<int>("intKey", 0));
            Assert.AreEqual(5, args.GetValue<int>("INTKEY", 0));
        }

        [TestMethod()]
        public void AssetManager_Arguments_Equality_Test()
        {
            var actual = new AssetLoadArguments();
            actual.Add("Key", 5);

            var expected = new AssetLoadArguments();
            expected.Add("key", 5);
            Assert.AreEqual(expected, actual);

            expected = new AssetLoadArguments();
            expected.Add("Key", 5.0f);
            Assert.AreNotEqual(expected, actual);

            expected = new AssetLoadArguments();
            expected.Add("Key", "five");
            Assert.AreNotEqual(expected, actual);

            expected = new AssetLoadArguments();
            expected.Add("NotKey", 5);
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod()]
        public void AssetManager_Arguments_GetNonExistingValue_Test()
        {
            var args = new AssetLoadArguments();
            Assert.AreEqual(-9132, args.GetValue<int>("invalidKey", -9132));
        }

        public void AssetManager_Load_UnknownResource_Test()
        {
            var resMgr = new AssetManager(Device, RenderSettings.MediaPath);
            Assert.IsNull(resMgr.Load("Test/dummy.xxx") as Texture);
        }

        [TestMethod()]
        public void AssetManager_Cache_Test()
        {
            var resMgr = new AssetManager(Device, RenderSettings.MediaPath);

            string path1 = Path.Combine(RenderSettings.MediaPath, @"Test/DefaultColor.dds");
            string path2 = Path.Combine(RenderSettings.MediaPath, @"Test/../Test/DefaultColor.dds");
            Assert.AreEqual(Path.GetFullPath(path1), Path.GetFullPath(path2));

            Texture expected = resMgr.Load(path1) as Texture;
            Assert.AreEqual(expected, resMgr.Load(path2) as Texture);
        }

        [TestMethod()]
        public void AssetManager_IgnoreCase_Test()
        {
            var resMgr = new AssetManager(Device, RenderSettings.MediaPath);
            Texture expected = resMgr.Load(TestHelpers.SampleTexturePath) as Texture;
            Assert.AreEqual(expected, resMgr.Load(TestHelpers.SampleTexturePath.ToUpper()));
        }

        [TestMethod()]
        public void AssetManager_DefaultAssets_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            Assert.IsNotNull(assetMgr.DefaultTexture);
            Assert.IsNotNull(assetMgr.DefaultMaterial);
            Assert.IsNotNull(assetMgr.DefaultFont);
        }

        [TestMethod()]
        public void AssetManager_GetName_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            string path = Path.Combine(RenderSettings.MediaPath, @"sub\sub2\a");
            Assert.AreEqual(@"sub\sub2\a", assetMgr.GetName(path));
        }

        [TestMethod()]
        public void AssetManager_GetFullPath_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            var expected = Path.Combine(RenderSettings.MediaPath, @"sub\sub2\a");
            Assert.AreEqual(expected, assetMgr.GetFullPath(@"sub\sub2\a"));
        }

        [TestMethod()]
        public void AssetManager_GetShortName_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            Assert.AreEqual("a", assetMgr.GetShortName(@"sub\sub2\a"));
        }

        private class AssetFactoryMock : IAssetFactory
        {
            public Type AssetType { get; set; }
            public string[] FileExtensions { get; set; }
            public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
            {
                return new AssetMock();
            }
        }

        private class AssetMock
        {
        }

        [TestMethod()]
        public void AssetManager_RegisterAssetFactory_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            var factory = new AssetFactoryMock()
            {
                AssetType = typeof(AssetMock),
                FileExtensions = new string[] {".assetMock"},
            };
            assetMgr.RegisterAssetFactory(factory);

            Assert.AreEqual(typeof(AssetMock), assetMgr.GetAssetType(".assetMock"));
        }

        [TestMethod(), ExpectedException(typeof(ArgumentException))]
        public void AssetManager_RegisterAssetWithSameFileExtension_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            assetMgr.RegisterAssetFactory(new AssetFactoryMock() { FileExtensions = new string[] {".a"} });
            assetMgr.RegisterAssetFactory(new AssetFactoryMock() { FileExtensions = new string[] {".a"} });
        }

        [TestMethod()]
        public void AssetManager_GetAssetType2WithNotRegisteredExtension_Test()
        {
            var assetMgr = new AssetManager(Device, RenderSettings.MediaPath);
            Assert.IsNull(assetMgr.GetAssetType("nonRegisteredFile.extension"));
        }
    }
}
