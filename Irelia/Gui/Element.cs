using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml;
using System.Linq;
using Irelia.Render;

namespace Irelia.Gui
{
    public enum ElementType
    {
        Custom,
        Layout,
        Image,
        TextBlock,
        Button
    }

    public abstract class Element : IElement, ISprite
    {
        #region Constructor
        protected Element(ElementType type, IElement parent, AssetManager assetManager)
        {
            Type = type;
            this.parent = parent;
            this.assetManager = assetManager;

            Name = GetType().Name;
            IsLogical = true;

            this.parent.PositionChanged += OnParentPositionChanged;
            this.parent.SizeChanged += OnParentSizeChanged;

            var parentElement = this.parent as Element;
            if (parentElement != null)
            {
                IsVisible = parentElement.IsVisible;
                parentElement.AddChild(this);
            }
        }
        #endregion

        #region IElement
        public Rectangle AbsRect
        {
            get
            {
                return new Rectangle(parent.AbsRect.Left + parent.AbsRect.Width * DestRect.Left,
                                     parent.AbsRect.Top + parent.AbsRect.Height * DestRect.Top,
                                     parent.AbsRect.Width * DestRect.Width,
                                     parent.AbsRect.Height * DestRect.Height);
            }
        }
        public event EventHandler PositionChanged = delegate { };
        public event EventHandler SizeChanged = delegate { };
        #endregion

        #region ISprite
        bool ISprite.Render(SpriteRenderer spriteRenderer)
        {
            if (IsVisible == false)
                return false;
            return OnRender(spriteRenderer);
        }
        #endregion

        #region Properties
        public string Name { get; set; }

        public bool IsLogical { get; set; }

        public virtual Rectangle DestRect
        {
            get { return this.destRect; }
            set
            {
                this.destRect = value;
                PositionChanged(this, EventArgs.Empty);
                SizeChanged(this, EventArgs.Empty);
            }
        }

        public ReadOnlyCollection<Element> Childs
        {
            get { return new ReadOnlyCollection<Element>(this.childs); }
        }

        public ElementType Type { get; private set; }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set
            {
                this.isVisible = value;
                LogicalChilds.ForEach(e => e.IsVisible = value);
            }
        }

        private List<Element> LogicalChilds
        {
            get
            {
                return this.childs.Where(e => e.IsLogical).ToList();
            }
        }
        #endregion

        #region Fields
        protected IElement parent;
        protected readonly AssetManager assetManager;

        private Rectangle destRect = new Rectangle(0.0f, 0.0f, 1.0f, 1.0f);
        private readonly List<Element> childs = new List<Element>();
        protected float lastMouseX = float.MinValue, lastMouseY = float.MinValue;
        protected bool isVisible = true;
        #endregion

        #region Methods
        protected abstract bool OnRender(SpriteRenderer spriteRenderer);
        protected abstract void OnWriteXml(XmlWriter writer);
        protected abstract object OnReadXml(XmlReader reader);
        protected virtual void OnReadXmlDone(object readContext) { }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(GetType().Name);

            writer.WriteAttributeString("Name", Name.ToString());
            writer.WriteAttributeString("DestRect", DestRect.ToString());
            OnWriteXml(writer);

            // Write child elements
            childs.ForEach(c => c.WriteXml(writer));

            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() != XmlNodeType.Element ||  reader.LocalName != GetType().Name)
                return;
         
            int depth = reader.Depth;

            Name = reader["Name"];
            DestRect = Rectangle.Parse(reader["DestRect"]);
            
            var readContext = OnReadXml(reader);
            reader.Read();  // \r\n

            // Read child elements
            while (true)
            {
                var contentType = reader.MoveToContent();
                if (contentType == XmlNodeType.EndElement)
                {
                    reader.ReadEndElement();
                    continue;
                }
                else if (contentType == XmlNodeType.Element && reader.Depth == depth + 1)
                {
                    string typeName = GetType().Namespace + "." + reader.LocalName;
                    var type = System.Type.GetType(typeName);
                    if (type != null && type.IsSubclassOf(typeof(Element)))
                    {
                        var child = type.Assembly.CreateInstance(typeName, false, BindingFlags.Default, null,
                            new object[] { this, this.assetManager }, // constructor args
                            null, null) as Element;
                        child.ReadXml(reader);
                        AddChild(child);
                    }
                    else
                    {
                        break;  // end reading childs
                    }
                }
                else
                {
                    break;
                }
            }

            OnReadXmlDone(readContext);
        }

        public bool RemoveFromParent()
        {
            var parent = this.parent as Element;
            if (parent == null || parent.RemoveChild(this) == false)
                return false;

            parent.PositionChanged -= OnParentPositionChanged;
            parent.SizeChanged -= OnParentSizeChanged;

            this.parent = null;
            return true;
        }

        public List<ISprite> GetVisualTree()
        {
            var visualTree = new List<ISprite>();

            Action<Element> traverse = null;
            traverse = (element) => 
                {
                    visualTree.Add(element);
                    element.childs.ForEach(traverse);
                };

            traverse(this);
            return visualTree;
        }

        public void InjectMouseDownEvent(float x, float y, MouseButton mouseButton)
        {
            FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.MouseDown);

            if (mouseButton == MouseButton.Left)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.LeftDown);
            else if (mouseButton == MouseButton.Right)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.RightDown);
            else if (mouseButton == MouseButton.Middle)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.MiddleDown);
        }

        public void InjectMouseUpEvent(float x, float y, MouseButton mouseButton)
        {
            FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.MouseUp);

            if (mouseButton == MouseButton.Left)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.LeftUp);
            else if (mouseButton == MouseButton.Right)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.RightUp);
            else if (mouseButton == MouseButton.Middle)
                FireMouseDownUpEvent(x, y, mouseButton, MouseEventType.MiddleUp);
        }

        public void InjectMouseMoveEvent(float x, float y)
        {
            FireMouseMoveEvent(x, y);
            FireMouseEnterEvent(x, y);
            FireMouseLeaveEvent(x, y);

            this.lastMouseX = x;
            this.lastMouseY = y;
            LogicalChilds.ForEach(e => { e.lastMouseX = x; e.lastMouseY = y; });
        }

        public Element GetElement(string name)
        {
            return this.childs.Find(e => e.Name == name);
        }

        protected bool AddChild(Element child)
        {
            if (this.childs.Contains(child))
                return false;
            this.childs.Add(child);
            return true;
        }

        protected bool RemoveChild(Element child)
        {
            return this.childs.Remove(child);
        }

        protected bool PositionIsInElement(float x, float y)
        {
            return x >= AbsRect.Left && x <= AbsRect.Right && y >= AbsRect.Top && y <= AbsRect.Bottom;
        }

        protected void UpdateDestRect()
        {
            DestRect = DestRect;
        }

        private void FireMouseMoveEvent(float x, float y)
        {
            foreach (var child in LogicalChilds)
            {
                child.FireMouseMoveEvent(x, y);
            }

            if (PositionIsInElement(x, y))
            {
                MouseMove(this, new MouseEventArgs(x, y));
            }
        }

        private bool FireMouseEnterEvent(float x, float y)
        {
            foreach (var child in LogicalChilds)
            {
                if (child.FireMouseEnterEvent(x, y))
                    return true;
            }

            if (PositionIsInElement(lastMouseX, lastMouseY) == false &&
                PositionIsInElement(x, y))
            {
                var args = new MouseEventArgs(x, y);
                MouseEnter(this, args);
                return args.Handled;
            }
            else
            {
                return false;
            }
        }

        private bool FireMouseLeaveEvent(float x, float y)
        {
            foreach (var child in LogicalChilds)
            {
                if (child.FireMouseLeaveEvent(x, y))
                    return true;
            }

            if (PositionIsInElement(lastMouseX, lastMouseY) &&
                PositionIsInElement(x, y) == false)
            {
                var args = new MouseEventArgs(x, y);
                MouseLeave(this, args);
                return args.Handled;
            }
            else
            {
                return false;
            }
        }

        private void OnParentPositionChanged(object sender, EventArgs e)
        {
            UpdateDestRect();
            PositionChanged(this, EventArgs.Empty);
        }

        private void OnParentSizeChanged(object sender, EventArgs e)
        {
            UpdateDestRect();
            SizeChanged(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion

        #region Events
        public event EventHandler<MouseButtonEventArgs> MouseDown = delegate { };               // direct
        public event EventHandler<MouseButtonEventArgs> MouseLeftButtonDown = delegate { };     // direct
        public event EventHandler<MouseButtonEventArgs> MouseRightButtonDown = delegate { };    // direct
        public event EventHandler<MouseButtonEventArgs> MouseMiddleButtonDown = delegate { };    // direct
        public event EventHandler<MouseButtonEventArgs> MouseUp = delegate { };                 // direct
        public event EventHandler<MouseButtonEventArgs> MouseLeftButtonUp = delegate { };       // direct
        public event EventHandler<MouseButtonEventArgs> MouseRightButtonUp = delegate { };      // direct
        public event EventHandler<MouseButtonEventArgs> MouseMiddleButtonUp = delegate { };    // direct
        public event EventHandler<MouseEventArgs> MouseMove = delegate { };     // bubbling
        public event EventHandler<MouseEventArgs> MouseEnter = delegate { };    // direct
        public event EventHandler<MouseEventArgs> MouseLeave = delegate { };    // direct
        #endregion

        #region Private Enum
        private enum MouseEventType
        {
            MouseDown,
            LeftDown,
            RightDown,
            MiddleDown,
            MouseUp,
            LeftUp,
            RightUp,
            MiddleUp,
        }
        #endregion

        #region Private Methods
        private bool FireMouseDownUpEvent(float x, float y, MouseButton mouseButton, MouseEventType eventType)
        {
            foreach (var child in LogicalChilds)
            {
                if (child.FireMouseDownUpEvent(x, y, mouseButton, eventType))
                    return true;
            }

            if (PositionIsInElement(x, y))
            {
                var args = new MouseButtonEventArgs(x, y, mouseButton);
                switch (eventType)
                {
                    case MouseEventType.MouseDown:
                        MouseDown(this, args);
                        break;
                    case MouseEventType.LeftDown:
                        MouseLeftButtonDown(this, args);
                        break;
                    case MouseEventType.RightDown:
                        MouseRightButtonDown(this, args);
                        break;
                    case MouseEventType.MiddleDown:
                        MouseMiddleButtonDown(this, args);
                        break;
                    case MouseEventType.MouseUp:
                        MouseUp(this, args);
                        break;
                    case MouseEventType.LeftUp:
                        MouseLeftButtonUp(this, args);
                        break;
                    case MouseEventType.RightUp:
                        MouseRightButtonUp(this, args);
                        break;
                    case MouseEventType.MiddleUp:
                        MouseMiddleButtonUp(this, args);
                        break;
                }
                return args.Handled;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
