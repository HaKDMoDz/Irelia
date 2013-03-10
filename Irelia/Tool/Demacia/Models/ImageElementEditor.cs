using System.ComponentModel;
using Demacia.Views;
using Irelia.Render;
using Irelia.Gui;
using Microsoft.Windows.Controls.PropertyGrid.Attributes;

namespace Demacia.Models
{
    public sealed class ImageElementEditor : UIElementEditor
    {
        public ImageElementEditor(UIElementEditor parent, Element element, AssetManager assetManager)
            : base(parent, element, assetManager)
        {
        }

        private Image Image
        { 
            get { return Element as Image; }
        }

        [Category("Image")]
        [DisplayName("Texture")]
        [Editor(typeof(TextureTypeView), typeof(TextureTypeView))]
        public Texture Texture
        {
            get { return Image.Texture; }
            set { Image.Texture = value; }
        }

        [Category("Image")]
        [DisplayName("Source Rectangle")]
        public string SourceRectangle
        {
            get { return Image.SourceRect.ToString(); }
            set { Image.SourceRect = Rectangle.Parse(value); }
        }
    }
}
