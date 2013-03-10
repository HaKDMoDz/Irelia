using System.Windows;
using Demacia.ViewModels;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for ImportMeshView.xaml
    /// </summary>
    public partial class ImportMeshView : Window
    {
        public ImportMeshView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImportMeshViewModel viewModel = DataContext as ImportMeshViewModel;
            viewModel.RequestAccept += () => { DialogResult = true; };
            viewModel.RequestCancel += () => { DialogResult = false; };
        }
    }
}
