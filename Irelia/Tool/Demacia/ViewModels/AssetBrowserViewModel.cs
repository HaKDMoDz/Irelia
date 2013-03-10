using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Demacia.Models;
using Demacia.Utils;
using Irelia.Render;
using Demacia.Services;

namespace Demacia.ViewModels
{
    public class AssetBrowserViewModel : ViewModelBase
    {
        #region Inner class, AssetTypeFilter
        public class AssetTypeFilter : PropertyNotifier
        {
            public Type Type { get; set; }
            public string Name { get; set; }
            public bool IsChecked 
            {
                get { return this.isChecked; }
                set { this.isChecked = value; OnPropertyChanged("IsChecked"); }
            }

            private bool isChecked;
        }
        #endregion

        #region Properties
        public List<AssetFolder> AssetFolders { get; private set; }
        public ObservableCollection<Asset> ShownAssets { get; private set; }
        public Asset SelectedAsset { get; set; }
        public List<AssetTypeFilter> AssetTypeFilters { get; private set; }
        public string AssetNameFilter 
        {
            get { return this.assetNameFilter; }
            set { this.assetNameFilter = value; UpdateShownAssetsView(); }
        }
        #endregion

        #region Private fields
        private List<AssetFolder> selectedFolders = new List<AssetFolder>();
        private readonly IThumbnailManager thumbnailMgr = new ThumbnailManager();
        private string assetNameFilter = "";
        #endregion

        public AssetBrowserViewModel(string rootPath, Framework framework)
        {
            var rootFolder = new AssetFolder(rootPath, framework, this.thumbnailMgr);
            AssetFolders = new List<AssetFolder>(1) { rootFolder };

            ShownAssets = new ObservableCollection<Asset>();
            AssetFolders.ForEach((f) => AddToShownAssets(f));

            // Type filters
            AssetTypeFilters = framework.AssetManager.AllAssetTypes
                .Select((type) => new AssetTypeFilter() { Type = type, Name = type.Name, IsChecked = false }).ToList();
            AssetTypeFilters.Insert(0, new AssetTypeFilter() { Name = "All", IsChecked = true });
            AssetTypeFilters.ForEach((f) => f.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "IsChecked")
                    {
                        AssetTypeFilter filter = o as AssetTypeFilter;
                        if (filter.Name == "All" && filter.IsChecked == true)
                        {
                            AssetTypeFilters.Where((f2) => f2.Name != "All").ToList()
                                            .ForEach((f3) => f3.IsChecked = false);
                        }
                        else if (filter.Name != "All" && filter.IsChecked == true)
                        {
                            AssetTypeFilters.Find((f2) => f2.Name == "All").IsChecked = false;
                        }

                        UpdateShownAssetsView();
                    }
                });

            // Register all assets to service (for global access)
            AssetService.SetAssetFolders(AssetFolders);
        }

        public void SelectFolder(AssetFolder folder, bool clear)
        {
            if (clear)
            {
                this.selectedFolders.ForEach((f) => f.IsSelected = false);
                this.selectedFolders.Clear();
            }

            folder.IsSelected = true;
            this.selectedFolders.Add(folder);

            ShownAssets.Clear();
            this.selectedFolders.ForEach((f) => AddToShownAssets(f));
        }

        private void AddToShownAssets(AssetFolder folder)
        {
            folder.Assets.ForEach((asset) =>
            {
                if (ShownAssets.Contains(asset) == false)
                    ShownAssets.Add(asset);
            });
            folder.ChildFolders.ForEach((f) => AddToShownAssets(f));
        }

        private void UpdateShownAssetsView()
        {
            var view = CollectionViewSource.GetDefaultView(ShownAssets);
            if (view == null)
                return;

            view.Filter = (a) => 
                {
                    Asset asset = a as Asset;

                    if (asset.Name.Contains(AssetNameFilter, StringComparison.OrdinalIgnoreCase) == false)
                        return false;

                    if (AssetTypeFilters.Find((filter) => filter.Name == "All").IsChecked)
                        return true;

                    return AssetTypeFilters.Find((filter) => filter.Type == asset.AssetType).IsChecked;
                };
        }
    }
}
