using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Demacia.Models;
using Demacia.Services;
using Irelia.Render;
using Microsoft.Windows.Controls.PropertyGrid;
using Microsoft.Windows.Controls.PropertyGrid.Editors;

namespace Demacia.Views
{
    /// <summary>
    /// Interaction logic for ShaderTypeView.xaml
    /// </summary>
    public partial class ShaderTypeView : UserControl, ITypeEditor
    {
        #region Properties
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Shader), typeof(ShaderTypeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Shader Value
        {
            get { return (Shader)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public List<ShaderAsset> ShaderAssets { get { return AssetService.ShaderAssets; } }
        public ShaderAsset SelectedShaderAsset 
        {
            get { return this.selectedShaderAsset; }
            set { this.selectedShaderAsset = value; Value = this.selectedShaderAsset.Shader; }
        }
        #endregion

        private ShaderAsset selectedShaderAsset;

        public ShaderTypeView()
        {
            InitializeComponent();
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            Binding binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, ShaderTypeView.ValueProperty, binding);
            SelectedShaderAsset = ShaderAssets.Find((sa) => sa.Shader == propertyItem.Value as Shader);
            return this;
        }
    }
}
