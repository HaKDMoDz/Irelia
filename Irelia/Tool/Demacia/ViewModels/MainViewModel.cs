using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using AvalonDock;
using Demacia.Command;
using Demacia.Models;
using Demacia.Services;
using Demacia.Views;
using Irelia.Gui;
using Irelia.Render;

namespace Demacia.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Commands
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand ShowAssetBrowserCommand { get; private set; }
        public DelegateCommand ExecuteBitmapFontBuilderCommand { get; private set; }
        #endregion

        #region Properties
        public ErrorListViewModel ErrorListViewModel { get; private set; }
        public OutputViewModel OutputViewModel { get; private set; }
        public StatusBarViewModel StatusBarViewModel { get; private set; }
        public Framework Framework { get; private set; }
        public IList<Theme> Themes { get { return ThemeService.Themes; } }

        private DocumentContent AssetBrowserDoc
        {
            get
            {
                if (this.assetBrowserDoc == null)
                {
                    this.assetBrowserDoc = new DocumentContent()
                    {
                        Title = "Asset Browser",
                        Content = new AssetBrowserView() { DataContext = new AssetBrowserViewModel(RenderSettings.MediaPath, Framework) }
                    };
                }
                return this.assetBrowserDoc;
            }
        }
        #endregion

        private DocumentContent assetBrowserDoc;

        public MainViewModel()
        {
            var hwnd = new HwndSource(0, 0, 0, 0, 0, "HwndSource for MainViewModel", IntPtr.Zero);
            Framework = new Framework(hwnd.Handle, 0, 0, RenderSettings.MediaPath);
            Framework.AssetManager.RegisterAssetFactory(new LayoutFactory());

            // View-models
            ErrorListViewModel = new ErrorListViewModel();
            OutputViewModel = new OutputViewModel();
            StatusBarViewModel = new StatusBarViewModel();

            // Commands
            ExitCommand = new DelegateCommand(() => Application.Current.Shutdown());
            ShowAssetBrowserCommand = new DelegateCommand(() => DocumentService.ShowDocument(AssetBrowserDoc, false));
            ExecuteBitmapFontBuilderCommand = new DelegateCommand(() => Process.Start(Settings.BitmapFontBuilderPath));

            // Services
            StatusBarService.SetStatusBarViewModel(StatusBarViewModel);

            StatusBarService.StatusText = "Ready";
        }
    }
}