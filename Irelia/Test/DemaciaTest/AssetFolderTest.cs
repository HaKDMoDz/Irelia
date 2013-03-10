using Demacia.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows.Interop;
using Irelia.Render;
using System.Linq;

namespace DemaciaTest
{
    [TestClass()]
    public class AssetFolderTest
    {
        [TestInitialize()]
        public void SetUp()
        {
            var hwnd = new HwndSource(0, 0, 0, 0, 0, "HwndSource for Framework", IntPtr.Zero);
            Framework = new Framework(hwnd.Handle, 0, 0, RenderSettings.MediaPath);
        }

        private Framework Framework { get; set; }
        
        [TestMethod()]
        public void AssetFolder_Constructor_Test()
        {
            var folder = new AssetFolder(Path.Combine(RenderSettings.MediaPath, @"Test"), Framework, new ThumbnailManagerMock());

            Assert.IsTrue(folder.FullPath.EndsWith(@"Test"));
            Assert.AreEqual("Test", folder.Name);
            Assert.AreEqual(1, folder.ChildFolders.Count);
            Assert.IsTrue(folder.Assets.Count > 0);
            Assert.AreEqual(0, folder.Assets.Count((a) => a.Name == @"Test\Dummy.xxx"));
            Assert.AreEqual(1, folder.Assets.Count((a) => a.Name == @"Test\explosion_30_128.tga"));

            var subFolder = folder.ChildFolders[0];
            Assert.AreEqual("SubFolder", subFolder.Name);
            Assert.AreEqual(0, subFolder.ChildFolders.Count);
            Assert.AreEqual(1, subFolder.Assets.Count);
            Assert.AreEqual(@"Test\SubFolder\mountains_back.jpg", subFolder.Assets[0].Name);
        }
    }
}
