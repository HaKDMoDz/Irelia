using System.Collections.Generic;
using System.Windows.Input;
using Demacia.Command;
using Demacia.Services;
using Demacia.ViewModels;
using Demacia.Views;
using Irelia.Render;
using System;
using Irelia.Gui;

namespace Demacia.Models
{
    public sealed class LayoutAsset : Asset
    {
        #region Properties
        public Layout Layout
        {
            get
            {
                if (this.layout == null)
                {
                    this.layout = this.framework.AssetManager.Load(Name) as Layout;
                    OnPropertyChanged("IsLoaded");
                }
                return this.layout;
            }
        }

        public LayoutEditorViewModel LayoutEditorViewModel
        {
            get
            {
                if (this.layoutEditorViewModel == null)
                    this.layoutEditorViewModel = new LayoutEditorViewModel(this, this.framework);
                return this.layoutEditorViewModel;
            }
        }

        public override bool IsLoaded
        {
            get { return this.layout != null; }
        }
        #endregion

        #region Fields
        private LayoutEditorViewModel layoutEditorViewModel;
        private Layout layout;
        #endregion

        #region Commands
        public override ICommand OpenCommand
        {
            get { return this.openCommand; }
        }
        private readonly DelegateCommand openCommand;
        #endregion

        public LayoutAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(Layout), name, framework, thumbnailMgr)
        {
            this.openCommand = new DelegateCommand(OpenEditor);
            Operations.Add(new Operation() { Name = "Edit Using Layout Editor...", Command = OpenCommand });
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            Layout.Size = renderTarget.Size;
            return LayoutEditorViewModel.SceneManager.Render(this.framework.Renderer, renderTarget, false);
        }

        private void OpenEditor()
        {
            var doc = new LayoutEditorView()
            {
                Title = "Layout Editor: " + Name,
                DataContext = LayoutEditorViewModel
            };
            DocumentService.ShowDocument(doc, false);
        }
    }
}
