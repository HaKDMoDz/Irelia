using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for TextureControlEditor.xaml
    /// </summary>
    public partial class TextureTypeView : UserControl, ITypeEditor
    {
        #region Properties
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Texture), typeof(TextureTypeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Texture Value
        {
            get { return (Texture)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public List<TextureAsset> TextureAssets { get { return AssetService.TextureAssets; } }
        public TextureAsset SelectedTextureAsset 
        {
            get { return this.selectedTextureAsset; }
            set { this.selectedTextureAsset = value; Value = this.selectedTextureAsset.Texture; }
        }
        #endregion

        private TextureAsset selectedTextureAsset;

        public TextureTypeView()
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
            BindingOperations.SetBinding(this, TextureTypeView.ValueProperty, binding);

            if (propertyItem.Value != null)
                SelectedTextureAsset = TextureAssets.FirstOrDefault((ta) => ta.Texture == propertyItem.Value as Texture);

            return this;
        }
    }
}
