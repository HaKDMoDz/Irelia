using System.Windows;
using Demacia.ViewModels;
using Demacia.Services;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            Loaded += ((o, e) => 
                {
                    DocumentService.DockingManager = this.dockingManager;
                    (DataContext as MainViewModel).ShowAssetBrowserCommand.Execute();
                });
        }
    }
}
