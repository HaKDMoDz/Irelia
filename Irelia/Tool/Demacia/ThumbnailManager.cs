using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ComponentAce.Compression.ZipForge;
using System.IO;
using ComponentAce.Compression.Archiver;
using System.Windows.Media.Imaging;
using Irelia.Render;

namespace Demacia
{
    public interface IThumbnailManager
    {
        BitmapImage GetThumbnail(string name);
        void SetThumbnail(string name, BitmapImage thumbnail);
    }

    public class ThumbnailManager : IThumbnailManager
    {
        BitmapImage IThumbnailManager.GetThumbnail(string name)
        {
            try
            {
                using (var zipArchive = new ZipArchive("AssetThumbnails.zip"))
                {
                    var stream = zipArchive.Load(name);

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = stream;
                    bi.EndInit();
                    return bi;
                }
            }
            catch
            {
                return null;
            }
        }

        void IThumbnailManager.SetThumbnail(string name, BitmapImage thumbnail)
        {
            using (var zipArchive = new ZipArchive("AssetThumbnails.zip"))
            {
                zipArchive.Save(name, thumbnail.StreamSource);
            }
        }
    }
}
