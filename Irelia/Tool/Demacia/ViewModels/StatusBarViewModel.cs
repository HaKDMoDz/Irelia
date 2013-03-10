
namespace Demacia.ViewModels
{
    public sealed class StatusBarViewModel : ViewModelBase
    {
        public string StatusText
        {
            get { return this.statusText; }
            set { this.statusText = value; OnPropertyChanged("StatusText"); }
        }

        private string statusText;
    }
}
