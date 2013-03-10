using Irelia.Gui;
using Irelia.Render;
using System.ComponentModel;
using Microsoft.Windows.Controls.PropertyGrid.Attributes;

namespace Demacia.Models
{
    public sealed class ButtonElementEditor : UIElementEditor
    {
        private readonly Button button;

        public ButtonElementEditor(UIElementEditor parent, Element element, AssetManager assetManager)
            : base(parent, element, assetManager)
        {
            this.button = element as Button;
        }

        private string GetTemplateName(ButtonState state)
        {
            var template = this.button.GetTemplate(state);
            return (template != null) ? template.Name : "";
        }

        private void SetTemplateName(ButtonState state, string name)
        {
            this.button.SetTemplate(state, name);
        }

        [Category("Button")]
        [DisplayName("Normal Template")]
        public string NormalTemplate
        {
            get { return GetTemplateName(ButtonState.Normal); }
            set { SetTemplateName(ButtonState.Normal, value); }
        }

        [Category("Button")]
        [DisplayName("Hover Template")]
        public string HoverTemplate
        {
            get { return GetTemplateName(ButtonState.Hover); }
            set { SetTemplateName(ButtonState.Hover, value); }
        }

        [Category("Button")]
        [DisplayName("Pushed Template")]
        public string PushedTemplate
        {
            get { return GetTemplateName(ButtonState.Pushed); }
            set { SetTemplateName(ButtonState.Pushed, value); }
        }

        [Category("Button")]
        [DisplayName("Disabled Template")]
        public string DisabledTemplate
        {
            get { return GetTemplateName(ButtonState.Disabled); }
            set { SetTemplateName(ButtonState.Disabled, value); }
        }
    }
}
