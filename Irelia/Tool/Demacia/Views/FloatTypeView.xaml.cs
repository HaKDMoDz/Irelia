using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Windows.Controls.PropertyGrid;
using Microsoft.Windows.Controls.PropertyGrid.Editors;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for FloatTypeView.xaml
    /// </summary>
    public partial class FloatTypeView : UserControl, ITypeEditor
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(float), typeof(FloatTypeView), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public FloatTypeView()
        {
            InitializeComponent();
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, FloatTypeView.ValueProperty, binding);
            
            Value = (float)propertyItem.Value;
            
            return this;
        }
    }
}
