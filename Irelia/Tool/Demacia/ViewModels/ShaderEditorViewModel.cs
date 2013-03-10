using Irelia.Render;
using Demacia.Command;
using ICSharpCode.AvalonEdit.Document;
using System.IO;
using Demacia.Models;

namespace Demacia.ViewModels
{
    public class ShaderEditorViewModel : ViewModelBase
    {
        public DelegateCommand SaveCommand { get; private set; }
        public TextDocument Document { get; set; }

        private readonly Shader shader;
       
        public ShaderEditorViewModel(Shader shader)
        {
            this.shader = shader;
            SaveCommand = new DelegateCommand(Save);
            Document = new TextDocument(File.ReadAllText(shader.EffectFile));
        }

        private void Save()
        {
            using (var writer = new StreamWriter(this.shader.EffectFile))
            {
                writer.Write(Document.Text);
            }
            this.shader.Reload();
        }
    }
}
