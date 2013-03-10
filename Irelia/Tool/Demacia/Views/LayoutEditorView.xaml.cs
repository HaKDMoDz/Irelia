using System.Windows;
using System.Windows.Controls;
using AvalonDock;
using Demacia.Models;
using Demacia.ViewModels;

namespace Demacia.Views
{
    public partial class LayoutEditorView : DocumentContent
    {
        public LayoutEditorView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var layoutVM = DataContext as LayoutEditorViewModel;
            if (layoutVM == null)
                return;

            layoutVM.SelectedElement = e.NewValue as UIElementEditor;
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
