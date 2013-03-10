using System.ComponentModel;
using Irelia.Gui;
using Irelia.Render;
using Microsoft.Windows.Controls.PropertyGrid.Attributes;
using WM = System.Windows.Media;

namespace Demacia.Models
{
    public sealed class TextBlockElementEditor : UIElementEditor
    {
        public TextBlockElementEditor(UIElementEditor parent, Element element, AssetManager assetManager)
            : base(parent, element, assetManager)
        {
        }

        private TextBlock TextBlock
        {
            get { return this.Element as TextBlock; }
        }

        [Category("TextBlock")]
        [DisplayName("Text")]
        public string Text
        {
            get { return TextBlock.Text; }
            set { TextBlock.Text = value; }
        }

        [Category("TextBlock")]
        [DisplayName("Foreground")]
        public WM.Color Foreground
        {
            get { return TextBlock.Foreground.ToWindowsColor(); }
            set { TextBlock.Foreground = value.ToIreliaColor(); }
        }

        [Category("TextBlock")]
        [DisplayName("Position")]
        public string Position
        {
            get { return TextBlock.Position.ToString(); }
            set { TextBlock.Position = Vector2.Parse(value); }
        }

        [Category("TextBlock")]
        [DisplayName("Font")]
        [ItemsSource(typeof(FontItemsSource))]
        public Font Font
        {
            get { return TextBlock.Font; }
            set { TextBlock.Font = value; }
        }
    }
}
