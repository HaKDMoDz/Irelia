using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.ZipForge;
using Demacia.Command;
using Demacia.Services;
using Irelia.Render;
using D3D = SlimDX.Direct3D9;
using Irelia.Gui;
using System.Reflection;

namespace Demacia.Models
{
    public abstract class Asset : PropertyNotifier
    {
        public class Operation
        {
            public string Name { get; set; }
            public ICommand Command { get; set; }
        }

        #region Public properties
        public Type AssetType { get; private set; }
        public string ShortName { get; private set; }
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public IList<Operation> Operations { get; private set; }
        public System.Windows.Media.ImageSource Thumbnail 
        {
            get
            {
                if (this.thumbnail == null && this.thumbnailMgr != null)
                    this.thumbnail = this.thumbnailMgr.GetThumbnail(ThumbnailPath);

                if (this.thumbnail == null)
                {
                    UpdateThumbnail();
                }

                return this.thumbnail;
            }
        }
        public string ThumbnailPath { get; private set; }
        public long FileSize { get; private set; }
        #endregion

        #region Private fields
        private static readonly Size thumbnailSize = new Size(512, 512);
        protected readonly Framework framework;
        private readonly IThumbnailManager thumbnailMgr;
        private BitmapImage thumbnail;
        #endregion

        public static Asset Create(string name, Framework framework, IThumbnailManager thumbnailMgr)
        {
            Type type = framework.AssetManager.GetAssetType(name);
            if (type == null)
                return null;

            string assetTypeName = typeof(Asset).Namespace + "." + type.Name + "Asset";
            Type assetType = Type.GetType(assetTypeName);
            if (assetType == null)
                return null;

            return assetType.Assembly.CreateInstance(assetTypeName, false, BindingFlags.Default, null,
                                                     new object[] { name, framework, thumbnailMgr },
                                                     null, null) as Asset;
        }

        public Asset(Type assetType, string name, Framework framework, IThumbnailManager thumbnailMgr)
        {
            this.framework = framework;
            this.thumbnailMgr = thumbnailMgr;

            AssetType = assetType;
            Name = name;
            FullPath = this.framework.AssetManager.GetFullPath(name);
            ShortName = this.framework.AssetManager.GetShortName(name);
            ThumbnailPath = Name + ".jpg";

            if (File.Exists(FullPath))
            {
                FileSize = new FileInfo(FullPath).Length;
            }

            Operations = new List<Operation>()
            { 
                new Operation() { Name = "Update Thumbnail Image", Command = new DelegateCommand(() => UpdateThumbnail()) }
            };
        }

        private void UpdateThumbnail()
        {
            TextureRenderTarget renderTarget = new TextureRenderTarget(this.framework.Device, thumbnailSize)
            {
                ClearBackGround = true,
                ClearColor = Color.Black,
                ClearOptions = D3D.ClearFlags.Target | D3D.ClearFlags.ZBuffer
            };

            if (RenderThumbnail(renderTarget) == false)
                return;

            this.thumbnail = new BitmapImage();
            this.thumbnail.BeginInit();
            this.thumbnail.CacheOption = BitmapCacheOption.OnLoad;
            this.thumbnail.StreamSource = renderTarget.Texture.ToJpgStream();
            this.thumbnail.EndInit();

            if (this.thumbnailMgr != null)
                this.thumbnailMgr.SetThumbnail(ThumbnailPath, this.thumbnail);

            OnPropertyChanged("Thumbnail");
        }

        #region Virtuals & Abstracts
        public abstract ICommand OpenCommand { get; }

        protected abstract bool RenderThumbnail(RenderTarget renderTarget);

        public abstract bool IsLoaded { get; }
        #endregion
    }
}
