using System.Windows;
using System.Windows.Controls;
using Demacia.ViewModels;
using Demacia.Models;
using System.Windows.Input;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for ContentBrowserView.xaml
    /// </summary>
    public partial class AssetBrowserView : UserControl
    {
        public AssetBrowserView()
        {
            InitializeComponent();

            Loaded += AssetBrowserView_Loaded;
        }

        void AssetBrowserView_Loaded(object sender, RoutedEventArgs e)
        {
            this.assetNameFilter.Focus();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var folder = e.NewValue as AssetFolder;
            if (folder == null)
                return;

            var model = DataContext as AssetBrowserViewModel;
            bool ctrlPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            model.SelectFolder(folder, !ctrlPressed);
        }
    }
}
