using System.IO;
using System.Text;
using Irelia.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RenderTest
{
    [TestClass()]
    public class ZipArchiveTest
    {
        [TestMethod()]
        public void ZipArchive_Load_Test()
        {
            var zip = new ZipArchive(TestHelpers.GetAssetFullPath("ZipTest.zip"));
            var stream = zip.Load("Text/TxtFile.txt");
            Assert.IsNotNull(stream);

            byte[] bytes = new byte[stream.Length];
            int length = stream.Read(bytes, 0, bytes.Length);
            Assert.AreEqual("Content", Encoding.Default.GetString(bytes));
        }

        [TestMethod(), ExpectedException(typeof(ComponentAce.Compression.Archiver.ArchiverException))]
        public void ZipArchive_LoadFail_Test()
        {
            var zip = new ZipArchive(TestHelpers.GetAssetFullPath("ZipTest.zip"));
            zip.Load("NonExsting.file");
        }

        [TestMethod()]
        public void ZipArchive_Save_Test()
        {
            var zip = new ZipArchive("new.zip");
            string expected = "Content";
            zip.Save("binary.bin", new MemoryStream(Encoding.Default.GetBytes(expected)));

            var stream = zip.Load("binary.bin");
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            Assert.AreEqual("Content", Encoding.Default.GetString(bytes));
        }
    }
}
