using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Demacia.Command;
using Demacia.Models;
using Demacia.Services;

namespace Demacia.ViewModels
{
    public sealed class ErrorListViewModel : ViewModelBase
    {
        #region Commands
        private DelegateCommand clearAllCommand;
        public ICommand ClearAllCommand
        {
            get
            {
                if (this.clearAllCommand == null)
                    this.clearAllCommand = new DelegateCommand(() => Logs.Clear(), () => Logs.Count > 0);
                return this.clearAllCommand;
            }
        }
        #endregion

        #region Private Fiels
        private bool showVerboses = true;
        private bool showInfos = true;
        private bool showWarnings = true;
        private bool showErrors = true;
        #endregion

        #region Properties
        public ObservableCollection<LogData> Logs { get; private set; }

        public int NumVerboses
        {
            get
            {
                return Logs.Count((l) => l.Level == TraceLevel.Verbose);
            }
        }

        public int NumInfos
        {
            get
            {
                return Logs.Count((l) => l.Level == TraceLevel.Info);
            }
        }

        public int NumWarnings
        {
            get
            {
                return Logs.Count((l) => l.Level == TraceLevel.Warning);
            }
        }

        public int NumErrors
        {
            get
            {
                return Logs.Count((l) => l.Level == TraceLevel.Error);
            }
        }

        public bool ShowVerboses
        {
            get { return this.showVerboses; }
            set { this.showVerboses = value; OnPropertyChanged("ShowVerboses"); UpdateFilter(); }
        }

        public bool ShowInfos
        {
            get { return this.showInfos; }
            set { this.showInfos = value; OnPropertyChanged("ShowInfos"); UpdateFilter(); }
        }

        public bool ShowWarnings
        {
            get { return this.showWarnings; }
            set { this.showWarnings = value; OnPropertyChanged("ShowWarnings"); UpdateFilter(); }
        }

        public bool ShowErrors
        {
            get { return this.showErrors; }
            set { this.showErrors = value; OnPropertyChanged("ShowErrors"); UpdateFilter(); }
        }
        #endregion

        public ErrorListViewModel()
        {
            Logs = new ObservableCollection<LogData>();

            UpdateFilter();

            Logs.CollectionChanged += (s, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Move)
                {
                    OnPropertyChanged("NumErrors");
                    OnPropertyChanged("NumWarnings");
                    OnPropertyChanged("NumInfos");
                    OnPropertyChanged("NumVerboses");
                }
            };

            LogService.Logged += ((o, e) =>
                {
                    this.Logs.Add(new LogData(DateTime.Now, e.Level, e.Message, e.Category));
                });
        }

        void DefaultLog_MessageLogged(string message, TraceLevel level, bool maskDebug, string logName)
        {
            this.dispatcher.Invoke((Action)(() =>
            {
                this.Logs.Add(new LogData(DateTime.Now, level, message, logName));
            }));
        }

        private void UpdateFilter()
        {
            ICollectionView logsView = CollectionViewSource.GetDefaultView(Logs);
            logsView.Filter = (object o) =>
            {
                LogData log = o as LogData;
                if (ShowErrors && log.Level == TraceLevel.Error)
                    return true;
                if (ShowWarnings && log.Level == TraceLevel.Warning)
                    return true;
                if (ShowInfos && log.Level == TraceLevel.Info)
                    return true;
                if (ShowVerboses && log.Level == TraceLevel.Verbose)
                    return true;
                return false;
            };
        }
    }
}
