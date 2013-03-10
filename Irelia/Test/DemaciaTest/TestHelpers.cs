using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Demacia;

namespace DemaciaTest
{
    internal class ThumbnailManagerMock : IThumbnailManager
    {
        IDictionary<string, BitmapImage> thumbnails = new Dictionary<string, BitmapImage>();

        void IThumbnailManager.SetThumbnail(string name, BitmapImage thumbnail)
        {
            this.thumbnails.Add(name, thumbnail);
        }

        BitmapImage IThumbnailManager.GetThumbnail(string name)
        {
            if (this.thumbnails.ContainsKey(name) == false)
                return null;

            return this.thumbnails[name];
        }
    }

    internal static class TestHelpers
    {
    }
}
