using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irelia.Render;
using System.Windows.Input;
using Demacia.Command;
using Irelia.Gui;

namespace Demacia.Models
{
    public sealed class FontAsset : Asset
    {
        public Font Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = this.framework.AssetManager.Load(Name) as Font;
                    OnPropertyChanged("IsLoaded");
                }
                return this.font;
            }
        }

        private Font font;

        public FontAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Font), name, framework, thumbnailMgr)
        {
        }

        #region Overrides Asset
        public override ICommand OpenCommand
        {
            get { return null; }
        }

        public override bool IsLoaded
        {
            get { return this.font != null; }
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            var fontScale = 1.0f;
            var printPos = new Vector2(0.0f, renderTarget.Size.Height / 2.0f - Font.CharHeight * fontScale);
            var sampleSprite = new FontSampleSprite(Font, "The quick brown fox jumps over the lazy dog. 1234567890", Color.White, printPos, fontScale);

            return this.framework.Renderer.Render(renderTarget, null, new List<ISprite>() { sampleSprite }, new Camera(), new Light(), false);
        }
        #endregion

        #region Inner class, 
        private class FontSampleSprite : ISprite
        {
            private readonly Font font;
            private readonly string sampleText;
            private readonly Color textColor;
            private readonly Vector2 printPos;
            private readonly float scale;

            public FontSampleSprite(Font font, string sampleText, Color textColor, Vector2 printPos, float scale)
            {
                this.font = font;
                this.sampleText = sampleText;
                this.textColor = textColor;
                this.printPos = printPos;
                this.scale = scale;
            }

            bool ISprite.Render(SpriteRenderer spriteRenderer)
            {
                return this.font.Print(this.sampleText, printPos, textColor, spriteRenderer, scale);
            }           
        }
        #endregion
    }
}
