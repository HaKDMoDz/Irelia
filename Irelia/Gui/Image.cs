using System;
using System.Xml;
using Irelia.Render;

namespace Irelia.Gui
{
    public sealed class Image : Element
    {
        #region Private Fields
        private Sprite sprite;
        private Rectangle sourceRect;
        #endregion

        #region Overrides Element
        public override Rectangle DestRect
        {
            set
            {
                base.DestRect = value;
                if (this.sprite != null)
                {
                    this.sprite.Position = AbsRect.Location;
                    
                    float widthTarget = this.parent.AbsRect.Width * value.Width;
                    float heightTarget = this.parent.AbsRect.Height * value.Height;
                    this.sprite.Scale = new Vector2(widthTarget / this.sprite.SourceRect.Width, heightTarget / this.sprite.SourceRect.Height);
                }
            }
        }

        protected override object OnReadXml(XmlReader reader)
        {
            string textureName = reader["Source"];
            if (String.IsNullOrWhiteSpace(textureName) == false)
                Texture = this.assetManager.Load(textureName) as Texture;
            SourceRect = Rectangle.Parse(reader["SourceRect"]);
            return null;
        }

        protected override void OnWriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Source", Texture != null? Texture.Name : "");
            writer.WriteAttributeString("SourceRect", SourceRect.ToString());
        }

        protected override bool OnRender(SpriteRenderer spriteRenderer)
        {
            if (this.sprite == null)
                return false;

            return (this.sprite as ISprite).Render(spriteRenderer);
        }
        #endregion

        #region Properties
        public Texture Texture 
        {
            get 
            {
                return (this.sprite != null) ? this.sprite.Texture : null;
            }
            set 
            { 
                this.sprite = new Sprite(value);
                UpdateDestRect();
                SourceRect = SourceRect;
            }
        }

        public Rectangle SourceRect
        {
            get 
            {
                return this.sourceRect;
            }
            set
            {
                this.sourceRect = value;

                if (this.sprite != null)
                {
                    this.sprite.SourceRect = value;
                    UpdateDestRect();
                }
            }
        }
        #endregion

        #region Constructors
        public Image(IElement parent, AssetManager assetManager)
            : base(ElementType.Image, parent, assetManager)
        {
        }
        #endregion
    }
}
