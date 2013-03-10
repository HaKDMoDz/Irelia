using System.Xml;
using Irelia.Render;

namespace Irelia.Gui
{
    public sealed class TextBlock : Element
    {
        #region Properties
        public Font Font { get; set; }
        public string Text { get; set; }
        public Color Foreground { get; set; }
        public Vector2 Position { get; set; }
        #endregion

        #region Constructors
        public TextBlock(IElement parent, AssetManager assetManager)
            : base(ElementType.TextBlock, parent, assetManager)
        {
            Text = "";
            Foreground = Color.Black;
        }
        #endregion

        #region Overrides Element
        protected override object OnReadXml(XmlReader reader)
        {
            string fontName = reader["Font"];
            if (string.IsNullOrWhiteSpace(fontName) == false)
                Font = this.assetManager.Load(fontName) as Font;
            Text = reader["Text"];
            Foreground = Color.Parse(reader["Foreground"]);
            Position = Vector2.Parse(reader["Position"]);
            return null;
        }

        protected override void OnWriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Font", Font != null ? Font.Name : "");
            writer.WriteAttributeString("Text", Text);
            writer.WriteAttributeString("Foreground", Foreground.ToString());
            writer.WriteAttributeString("Position", Position.ToString());
        }

        protected override bool OnRender(SpriteRenderer spriteRenderer)
        {
            if (Font == null || string.IsNullOrEmpty(Text))
                return true;

            var absPos = AbsRect.Location + new Vector2(AbsRect.Width * Position.x, AbsRect.Height * Position.y);
            return Font.Print(Text, absPos, Foreground, spriteRenderer);
        }
        #endregion
    }
}
