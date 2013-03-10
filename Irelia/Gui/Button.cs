using System.Xml;
using Irelia.Render;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace Irelia.Gui
{
    public enum ButtonState
    {
        Normal,
        Hover,
        Pushed,
        Disabled,
        States
    }

    public sealed class Button : Element
    {

        #region Properties
        public ButtonState State
        {
            get { return this.state; }
            set
            {
                this.state = value;

                this.templates.Where(kv => kv.Value != ActiveTemplate && kv.Value != null)
                              .Select(kv => kv.Value).ToList()
                              .ForEach(e => e.IsVisible = false);

                if (ActiveTemplate != null)
                    ActiveTemplate.IsVisible = true;
            }
        }

        private Element ActiveTemplate
        {
            get
            {
                var template = this.templates[State];
                if (template == null)
                {
                    if (PositionIsInElement(this.lastMouseX, this.lastMouseY))
                        template = this.templates[ButtonState.Hover];
                    else
                        template = this.templates[ButtonState.Normal];
                }
                return template;
            }
        }
        #endregion

        #region Fields
        private IDictionary<ButtonState, Element> templates = new Dictionary<ButtonState, Element>();
        private ButtonState state = ButtonState.Normal;
        #endregion

        #region Constructors
        public Button(IElement parent, AssetManager assetManager)
            : base(ElementType.Button, parent, assetManager)
        {
            foreach (ButtonState state in Enum.GetValues(typeof(ButtonState)))
            {
                this.templates.Add(state, null);
            }

            MouseEnter += ((o, e) => State = ButtonState.Hover);
            MouseLeave += ((o, e) => State = ButtonState.Normal);
            MouseLeftButtonDown += ((o, e) =>
                {
                    State = ButtonState.Pushed;
                    e.Handled = true;
                });
            MouseLeftButtonUp += ((o, e) =>
                {
                    if (PositionIsInElement(e.Position.x, e.Position.y))
                        State = ButtonState.Hover;
                    else
                        State = ButtonState.Normal;
                    e.Handled = true;
                });
        }
        #endregion

        #region Public Methods
        public bool SetTemplate(ButtonState state, string childName)
        {
            if (this.templates[state] != null)
                this.templates[state].IsLogical = true;

            if (childName == null)
                return true;

            this.templates[state] = Childs.FirstOrDefault(e => e.Name == childName);
            if (this.templates[state] != null)
                this.templates[state].IsLogical = false;

            State = State;
            return this.templates[state] != null;
        }

        public Element GetTemplate(ButtonState state)
        {
            return this.templates[state];
        }
        #endregion

        #region Overrides Element
        protected override object OnReadXml(XmlReader reader)
        {
            var templates = new Dictionary<ButtonState, string>();
            foreach (ButtonState state in Enum.GetValues(typeof(ButtonState)))
            {
                templates.Add(state, reader[state.ToString()]);
            }
            return templates;
        }

        protected override void OnWriteXml(XmlWriter writer)
        {
            foreach (ButtonState state in Enum.GetValues(typeof(ButtonState)))
            {
                if (this.templates[state] != null)
                    writer.WriteAttributeString(state.ToString(), this.templates[state].Name);
                else
                    writer.WriteAttributeString(state.ToString(), "");
            }
        }

        protected override void OnReadXmlDone(object readContext)
        {
            var templates = readContext as IDictionary<ButtonState, string>;
            foreach (var template in templates)
            {
                SetTemplate(template.Key, template.Value);
            }
        }

        protected override bool OnRender(SpriteRenderer spriteRenderer)
        {
            var template = this.templates[State];
            if (template == null)
                template = this.templates[ButtonState.Normal];
            
            if (template == null)
                return false;

            return (template as ISprite).Render(spriteRenderer);
        }
        #endregion
    }
}
