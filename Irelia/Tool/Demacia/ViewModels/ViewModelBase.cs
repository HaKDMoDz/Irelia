using System.Windows;
using System.Windows.Threading;

namespace Demacia.ViewModels
{
    /// <summary>
    /// Provides common functionality for ViewModel classes
    /// </summary>
    public abstract class ViewModelBase : PropertyNotifier
    {
        protected readonly Dispatcher dispatcher;

        protected ViewModelBase()
        {
            if (Application.Current != null)
                this.dispatcher = Application.Current.Dispatcher;
            else
                this.dispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}
