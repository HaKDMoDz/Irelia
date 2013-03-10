using Demacia.Services;
using Demacia.Command;
using System;

namespace Demacia.ViewModels
{
    public sealed class OutputViewModel : ViewModelBase
    {
        public string OutputTexts
        {
            get { return this.outputTexts; }
            set
            {
                this.outputTexts = value;
                OnPropertyChanged("OutputTexts");
            }
        }

        public DelegateCommand ClearAllCommand { get; private set; }

        private string outputTexts = "";

        public OutputViewModel()
        {
            OutputTexts = "";
            ClearAllCommand = new DelegateCommand(() => OutputTexts = "", () => OutputTexts.Length != 0);

            LogService.Logged += ((o, e) => OutputTexts += e.Message + Environment.NewLine);
        }
    }
}
