using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Irelia.Gui;
using Microsoft.Windows.Controls.PropertyGrid.Attributes;
using Irelia.Render;
using System.Windows.Input;
using System.Collections.Generic;
using Demacia.Command;

namespace Demacia.Models
{
    public class UIElementEditor
    {
        public class Operation
        {
            public string Name { get; set; }
            public ICommand Command { get; set; }
        }

        #region Public Properties
        public Element Element { get; private set; }
        public ObservableCollection<UIElementEditor> Childs { get; private set; }

        public IList<Operation> Operations { get; private set; }
        #endregion

        #region Events
        public EventHandler<EventArgs> TreeChanged = delegate { };
        #endregion

        private readonly AssetManager assetManager;
        private readonly UIElementEditor parent;

        public static UIElementEditor Create(UIElementEditor parent, Element element, AssetManager assetManager)
        {
            if (element.Type == ElementType.Custom || element.Type == ElementType.Layout)
                return new UIElementEditor(parent, element, assetManager);
            else if (element.Type == ElementType.Image)
                return new ImageElementEditor(parent, element, assetManager);
            else if (element.Type == ElementType.TextBlock)
                return new TextBlockElementEditor(parent, element, assetManager);
            else if (element.Type == ElementType.Button)
                return new ButtonElementEditor(parent, element, assetManager);
            else
                throw new ArgumentException("unknown element type: " + element.ToString());
        }

        #region Constructors
        protected UIElementEditor(UIElementEditor parent, Element element, AssetManager assetManager)
        {
            this.parent = parent;
            Element = element;
            this.assetManager = assetManager;

            Childs = new ObservableCollection<UIElementEditor>(element.Childs.ToList().Select(e => UIElementEditor.Create(this, e, assetManager)));
            Childs.CollectionChanged += (o, e) => TreeChanged(this, EventArgs.Empty);
            
            TreeChanged += (o, e) =>
            {
                if (this.parent != null)
                    this.parent.TreeChanged(this, EventArgs.Empty);
            };

            Operations = new List<Operation>()
            {
                new Operation() { Name = "Delete", Command = new DelegateCommand(Delete, CanDelete) },
                new Operation() { Name = "Add Image", Command = new DelegateCommand(AddImage) },
                new Operation() { Name = "Add TextBlock", Command = new DelegateCommand(AddTextBlock) },
                new Operation() { Name = "Add Button", Command = new DelegateCommand(AddButton) }
            };
        }
        #endregion

        #region Private Methods
        private void Delete()
        {
            Element.RemoveFromParent();
            this.parent.Childs.Remove(this);
        }

        private bool CanDelete()
        {
            return Element.Type != ElementType.Layout;
        }

        private void AddImage()
        {
            var img = new Image(Element, this.assetManager)
            {
                Name = "Image"
            };
            Childs.Add(UIElementEditor.Create(this, img, this.assetManager));
        }

        private void AddTextBlock()
        {
            var textBlock = new TextBlock(Element, this.assetManager)
            {
                Font = this.assetManager.DefaultFont
            };
            Childs.Add(UIElementEditor.Create(this, textBlock, this.assetManager));
        }

        private void AddButton()
        {
            var button = new Button(Element, this.assetManager);
            var textBlock = new TextBlock(button, assetManager) { Name = "NormalTemplate" };
            button.SetTemplate(ButtonState.Normal, textBlock.Name);
            Childs.Add(UIElementEditor.Create(this, button, this.assetManager));
        }
        #endregion

        [Category("Common")]
        [DisplayName("Name")]
        public string Name
        {
            get { return Element.Name; }
            set { Element.Name = value; }
        }

        [Category("Common")]
        [DisplayName("Destination Rectangle")]
        public string DestinationRectangle
        {
            get { return Element.DestRect.ToString(); }
            set { Element.DestRect = Rectangle.Parse(value); }
        }

        [Category("Common")]
        [DisplayName("Absolute Rectangle")]
        public string AbsoluteRectangle
        {
            get { return Element.AbsRect.ToString(); }
        }

        [Category("Common")]
        public bool IsVisible
        {
            get { return Element.IsVisible; }
            set { Element.IsVisible = value; }
        }
    }
}
