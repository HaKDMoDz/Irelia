using System.Collections.Generic;
using System.IO;
using System.Linq;
using Irelia.Render;
using Demacia.Services;

namespace Demacia.Models
{
    public class AssetFolder : PropertyNotifier
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public List<AssetFolder> ChildFolders { get; private set; }
        public List<Asset> Assets { get; private set; }
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        private bool isSelected;

        public AssetFolder(string path, Framework framework, IThumbnailManager thumbnailMgr)
        {
            FullPath = Path.GetFullPath(path);
            Name = Path.GetFileName(FullPath);

            string rootDir = framework.AssetManager.RootPath;

            // Sub folders
            var folders = Directory.GetDirectories(FullPath).ToList();
            folders.RemoveAll((f) => (File.GetAttributes(f) & FileAttributes.Hidden) == FileAttributes.Hidden);

            ChildFolders = new List<AssetFolder>(folders.Count);
            folders.ForEach((f) => ChildFolders.Add(new AssetFolder(f, framework, thumbnailMgr)));

            // Files
            var files = Directory.GetFiles(FullPath);
            Assets = new List<Asset>(files.Length);
            foreach (var file in files)
            {
                string name = FileService.GetRelativePath(rootDir + Path.DirectorySeparatorChar, file);
                var asset = Asset.Create(name, framework, thumbnailMgr);
                if (asset == null)
                    continue;

                Assets.Add(asset);
            }
        }
    }
}
