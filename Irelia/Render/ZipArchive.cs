using System.IO;
using Zip = ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;

namespace Irelia.Render
{
    public class ZipArchive : DisposableObject
    {
        private readonly Zip.ZipForge zip = new Zip.ZipForge();

        public ZipArchive(string zipFile)
        {
            this.zip.FileName = zipFile;
            this.zip.OpenArchive(FileMode.OpenOrCreate);
        }

        public Stream Load(string file)
        {
            var stream = new MemoryStream();
            this.zip.ExtractToStream(file, stream);
            stream.Position = 0;
            return stream;
        }

        public void Save(string file, Stream stream)
        {
            var item = new ArchiveItem();
            if (this.zip.FindFirst(file, ref item))
            {
                this.zip.DeleteFiles(file);
            }
            this.zip.AddFromStream(file, stream);
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                this.zip.CloseArchive();
                this.zip.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
