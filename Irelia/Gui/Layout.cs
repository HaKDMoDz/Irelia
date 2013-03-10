using System;
using System.Text;
using System.Xml;
using Irelia.Render;
using System.Collections.Generic;

namespace Irelia.Gui
{
    public sealed class Layout : Element
    {
        #region Inner-Class, RootElement
        private class RootElement : IElement
        {
            public Rectangle AbsRect
            {
                get { return this.absoluteRect; }
                set
                {
                    this.absoluteRect = value;
                    PositionChanged(this, EventArgs.Empty);
                    SizeChanged(this, EventArgs.Empty);
                }
            }

            public event EventHandler SizeChanged = delegate { };
            public event EventHandler PositionChanged = delegate { };

            private Rectangle absoluteRect = new Rectangle(0.0f, 0.0f, 0.0f, 0.0f);
        };
        #endregion

        #region Properties
        public Size Size
        {
            set
            {
                var root = this.parent as RootElement;
                root.AbsRect = new Rectangle(root.AbsRect.Location, value);
            }
        }
        #endregion

        public Layout(string name, AssetManager assetManager)
            : base(ElementType.Layout, new RootElement(), assetManager)
        {
            Name = name;
        }

        #region Overrides Element
        protected override object OnReadXml(XmlReader reader)
        {
            return null;
        }

        protected override void OnWriteXml(XmlWriter writer)
        {
        }

        protected override bool OnRender(SpriteRenderer spriteRenderer)
        {
            return true;
        }
        #endregion

        #region Save & Load
        public void Save(string fileName)
        {
            using (var xmlWriter = new XmlTextWriter(fileName, Encoding.Unicode))
            {
                xmlWriter.Formatting = Formatting.Indented;
                WriteXml(xmlWriter);
            }
        }

        public static Layout Load(string fileName, string name, AssetManager assetManager)
        {
            using (var xmlReader = new XmlTextReader(fileName))
            {
                var layout = new Layout(name, assetManager);
                layout.ReadXml(xmlReader);
                layout.Name = name;
                return layout;
            }
        }
        #endregion
    }

    public sealed class LayoutFactory : IAssetFactory
    {
        public string[] FileExtensions { get { return new string[] { ".layout" }; } }
        public Type AssetType { get { return typeof(Layout); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return Layout.Load(filePath, name, assetManager);
        }
    }
}
