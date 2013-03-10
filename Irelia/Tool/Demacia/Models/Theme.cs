using System;
using Demacia.Command;
using Demacia.Services;
using System.Windows.Input;

namespace Demacia.Models
{
    public class Theme : PropertyNotifier
    {
        private readonly DelegateCommand applyCommand;
        public ICommand ApplyCommand { get { return this.applyCommand; } }

        public string Name { get; private set; }
        public Uri Uri { get; private set; }
        public bool IsCurrent
        {
            get { return this.isCurrent; }
            set { this.isCurrent = value; OnPropertyChanged("IsCurrent"); }
        }

        private bool isCurrent;

        public Theme(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
            isCurrent = false;

            this.applyCommand = new DelegateCommand(() => ThemeService.ChangeTheme(this));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
