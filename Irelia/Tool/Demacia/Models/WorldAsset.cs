using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irelia.Render;
using System.Windows.Input;
using Demacia.Command;
using Demacia.Views;
using Demacia.ViewModels;
using Demacia.Services;

namespace Demacia.Models
{
    public sealed class WorldAsset : Asset
    {
        public World World
        {
            get
            {
                if (this.world == null)
                {
                    this.world = this.framework.AssetManager.Load(Name) as World;
                    OnPropertyChanged("IsLoaded");
                }
                return this.world;
            }
        }
        private World world;

        public WorldEditorViewModel WorldEditorViewModel
        {
            get
            {
                if (this.worldEditorViewModel == null)
                    this.worldEditorViewModel = new WorldEditorViewModel(this, this.framework);
                return this.worldEditorViewModel;
            }
        }
        private WorldEditorViewModel worldEditorViewModel;

        public override ICommand OpenCommand
        {
            get { return this.openCommand; }
        }
        private readonly DelegateCommand openCommand;

        public override bool IsLoaded
        {
            get { return this.world != null; }
        }

        public WorldAsset(string name, Framework framework, IThumbnailManager thumbnailMgr)
            : base(typeof(World), name, framework, thumbnailMgr)
        {
            this.openCommand = new DelegateCommand(OpenEditor);
            Operations.Add(new Operation() { Name = "Edit Using World Editor...", Command = OpenCommand });
        }

        protected override bool RenderThumbnail(RenderTarget renderTarget)
        {
            return WorldEditorViewModel.SceneManager.Render(framework.Renderer, renderTarget, false);
        }

        private void OpenEditor()
        {
            var doc = new WorldEditorView()
            {
                Title = "World Editor: " + Name,
                DataContext = WorldEditorViewModel
            };
            DocumentService.ShowDocument(doc, false);
        }
    }
}
