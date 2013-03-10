using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for ErrorListView.xaml
    /// </summary>
    public partial class ErrorListView : UserControl, INotifyPropertyChanged
    {
        private bool autoScroll;
        public bool AutoScroll
        {
            get { return this.autoScroll; }
            set
            {
                INotifyCollectionChanged collection = this.errorListView.ItemsSource as INotifyCollectionChanged;
                if (collection == null)
                    return;

                if (value == true)
                    collection.CollectionChanged += ErrorList_CollectionChanged;
                else
                    collection.CollectionChanged -= ErrorList_CollectionChanged;

                this.autoScroll = value;
                OnPropertyChanged("AutoScroll");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ErrorListView()
        {
            InitializeComponent();
        }

        private void ErrorList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // This code makes the listview scroll up-down and down-up in a infinite loop (in some systems?).
                //this.logStringsDisplay.ScrollIntoView(e.NewItems[0]);

                if (VisualTreeHelper.GetChildrenCount(this.errorListView) <= 0)
                    return;

                Decorator border = VisualTreeHelper.GetChild(this.errorListView, 0) as Decorator;
                ScrollViewer scroll = border.Child as ScrollViewer;
                scroll.ScrollToBottom();
            }
        }

        private bool firstLoaded = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstLoaded)
            {
                AutoScroll = true;
                firstLoaded = false;
            }
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
