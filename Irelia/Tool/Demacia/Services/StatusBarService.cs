using Demacia.ViewModels;

namespace Demacia.Services
{
    public static class StatusBarService
    {
        private static StatusBarViewModel statusBarViewModel { get; set; }

        public static void SetStatusBarViewModel(StatusBarViewModel vm)
        {
            statusBarViewModel = vm;
        }

        public static string StatusText
        {
            set { statusBarViewModel.StatusText = value; }
        }
    }
}
