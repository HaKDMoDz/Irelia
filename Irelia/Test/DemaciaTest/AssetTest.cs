using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Interop;
using Demacia.Models;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media.Imaging;
using Demacia;
using System.Windows.Media;
using Irelia.Gui;

namespace DemaciaTest
{
    [TestClass()]
    public class AssetTest
    {
        [TestInitialize()]
        public void SetUp()
        {
            var hwnd = new HwndSource(0, 0, 0, 0, 0, "HwndSource for Framework", IntPtr.Zero);
            Framework = new Framework(hwnd.Handle, 0, 0, RenderSettings.MediaPath);
        }

        private Framework Framework { get; set; }

        [TestMethod()]
        public void Asset_Create_Test()
        {
            Framework.AssetManager.RegisterAssetFactory(new LayoutFactory());

            var files = new Dictionary<string, Type>();
            files.Add(@"Test\SubFolder\mountains_back.jpg", typeof(TextureAsset));
            files.Add(@"Test\explosion_30_128.tga", typeof(TextureAsset));
            files.Add(@"Test\Default_color.dds", typeof(TextureAsset));
            files.Add(@"Test\Simple.fx", typeof(ShaderAsset));
            files.Add(@"Test\slime1.bmp", typeof(TextureAsset));
            files.Add(@"Test\tiger.x", typeof(DirectXMeshAsset));
            files.Add(@"Test\tiger.meshs", typeof(MeshAsset));
            files.Add(@"Test\tiger.mats", typeof(MaterialAsset));
            files.Add(@"Test\World.worlds", typeof(WorldAsset));
            files.Add(@"Test\Layout.layout", typeof(LayoutAsset));
            files.Add(@"Engine\system12.font", typeof(FontAsset));

            foreach (var file in files)
            {
                string name = file.Key;
                var assetType = file.Value;

                var asset = Asset.Create(name, Framework, new ThumbnailManagerMock());
                Assert.AreEqual(assetType, asset.GetType());
                Assert.AreEqual(name, asset.Name);
                Assert.IsTrue(asset.FullPath.EndsWith(name));
                Assert.IsNotNull(asset.Thumbnail);
                Assert.AreEqual(asset.Name + ".jpg", asset.ThumbnailPath);
            }
        }

        [TestMethod()]
        public void Asset_Create_UnknownAssetType_Fail_Test()
        {
            var asset = Asset.Create("Dummy.xxx", Framework, new ThumbnailManagerMock());
            Assert.IsNull(asset);
        }

        [TestMethod()]
        public void Asset_Thumbnail_Cache_Test()
        {
            IThumbnailManager mgr = new ThumbnailManagerMock();

            var asset = Asset.Create(@"Test\slime1.bmp", Framework, mgr);

            var thumbnail = new BitmapImage();
            mgr.SetThumbnail(asset.ThumbnailPath, thumbnail);

            Assert.AreEqual(thumbnail, asset.Thumbnail);
        }
    }
}
