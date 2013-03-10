using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Demacia.Utils
{
    public class ListViewColumnStretch : DependencyObject
    {
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached("Stretch", typeof(bool), typeof(ListViewColumnStretch), new UIPropertyMetadata(true, null, OnCoerceStretch));

        public static bool GetStretch(DependencyObject obj)
        {
            return (bool)obj.GetValue(StretchProperty);
        }

        public static void SetStretch(DependencyObject obj, bool value)
        {
            obj.SetValue(StretchProperty, value);
        }

        private static object OnCoerceStretch(DependencyObject source, object value)
        {
            ListView lv = source as ListView;

            if (lv == null)
                throw new ArgumentException("This property may only be used on ListViews");

            lv.Loaded += new RoutedEventHandler(lv_Loaded);
            lv.SizeChanged += new SizeChangedEventHandler(lv_SizeChanged);
            return value;
        }

        static void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.IsLoaded)
                SetColumnWidths(lv);
        }

        static void lv_Loaded(object sender, RoutedEventArgs e)
        {
            ListView lv = sender as ListView;
            SetColumnWidths(lv);
        }

        private static void SetColumnWidths(ListView listView)
        {
            GridView gridView = listView.View as GridView;
            if (gridView == null)
                return;

            //Pull the stretch columns fromt the tag property.
            List<GridViewColumn> columns = (listView.Tag as List<GridViewColumn>);
            double specifiedWidth = 0;

            if (columns == null)
            {
                //Instance if its our first run.
                columns = new List<GridViewColumn>();
                // Get all columns with no width having been set.
                foreach (GridViewColumn column in gridView.Columns)
                {
                    if (!(column.Width >= 0))
                        columns.Add(column);
                    else
                        specifiedWidth += column.ActualWidth;
                }
            }
            else
            {
                // Get all columns with no width having been set.
                foreach (GridViewColumn column in gridView.Columns)
                    if (!columns.Contains(column))
                        specifiedWidth += column.ActualWidth;
            }

            // Allocate remaining space equally.
            foreach (GridViewColumn column in columns)
            {
                //double newWidth = (listView.ActualWidth - specifiedWidth) / columns.Count;
                Decorator border = VisualTreeHelper.GetChild(listView, 0) as Decorator;
                ScrollViewer scrollViewer = border.Child as ScrollViewer;
                double scrollBarWidth = scrollViewer.ScrollableHeight > 0 ? SystemParameters.VerticalScrollBarWidth : 0;
                double newWidth = (listView.ActualWidth - scrollBarWidth - specifiedWidth) / columns.Count;
                if (newWidth >= 0)
                    column.Width = Math.Max(newWidth - 30, 0);
            }

            //Store the columns in the TAG property for later use.
            listView.Tag = columns;
        }
    }
}
