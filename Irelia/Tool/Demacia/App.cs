using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Demacia.Services;
using Demacia.ViewModels;
using Demacia.Views;
using Irelia.Render;

namespace Demacia
{
    class App : Application
    {
        [STAThread]
        public static void Main()
        {
#if DEBUG
            try
            {
#endif
                App app = new App();
                app.Startup += app_Startup;
                app.Exit += app_Exit;
                app.Run();
#if DEBUG
            }
            catch (Exception ex)
            {
                Log.Msg(TraceLevel.Error, ex.ToString());
                MessageBox.Show(ex.Message + "\r\n" + ex.ToString());
            }
#endif
        }

        static void app_Startup(object sender, StartupEventArgs e)
        {
            // Add resource dictionaries
            var dict = new ResourceDictionary()
            {
                Source = new Uri("Resources/SharedStyles.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(dict);

            // Set "MediaPath"
            string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../Media");
            RenderSettings.MediaPath = Path.GetFullPath(mediaPath);

            // Change theme
            ThemeService.ChangeTheme(ThemeService.Themes.First(theme => theme.Uri == Settings.Theme));

            // Show MainView
            MainView mainView = new MainView()
            {
                DataContext = new MainViewModel()
            };
            mainView.Show();
        }

        static void app_Exit(object sender, ExitEventArgs e)
        {
            Settings.Save();
        }
    }
}
