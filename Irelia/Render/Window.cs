using System;
using SlimDX.Windows;

namespace Irelia.Render
{
    public sealed class Window : DisposableObject
    {
        #region Member variables
        private RenderForm renderForm = new RenderForm();
        #endregion

        #region Properties
        public string Title
        {
            get { return renderForm.Text; }
        }

        public int Width
        {
            get { return renderForm.Width; }
        }

        public int Height
        {
            get { return renderForm.Height; }
        }

        public IntPtr Handle
        {
            get { return renderForm.Handle; }
        }

        public bool Created
        {
            get;
            private set;
        }

        public System.Windows.Forms.Form Form
        {
            get { return this.renderForm; }
        }
        #endregion

        public Window(string title, int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;

            renderForm.Text = title;
            renderForm.Width = width;
            renderForm.Height = height;
            Created = true;
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.renderForm != null)
                    this.renderForm.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}
