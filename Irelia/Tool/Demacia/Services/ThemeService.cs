using System;
using System.Collections.Generic;
using AvalonDock;
using Demacia.Models;

namespace Demacia.Services
{
    public static class ThemeService
    {
        public static Theme CurrentTheme;
        public static IList<Theme> Themes
        {
            get
            {
                if (themes == null)
                {
                    themes = new List<Theme>()
                    {
                        new Theme("Visual Studio 2010", new Uri("/AvalonDock.Themes;component/themes/dev2010.xaml", UriKind.RelativeOrAbsolute)),
                        new Theme("Expression Blend", new Uri("/AvalonDock.Themes;component/themes/ExpressionDark.xaml", UriKind.RelativeOrAbsolute)),
                        new Theme("Aero", new Uri("/AvalonDock;component/themes/aero.normalcolor.xaml", UriKind.RelativeOrAbsolute)),
                        new Theme("Luna", new Uri("/AvalonDock;component/themes/luna.normalcolor.xaml", UriKind.RelativeOrAbsolute)),
                        new Theme("Classic", new Uri("/AvalonDock;component/themes/classic.xaml", UriKind.RelativeOrAbsolute)),
                        new Theme("System Default", null)
                    };
                }
                return themes;
            }
        }

        public static void ChangeTheme(Theme theme)
        {
            if (CurrentTheme != null)
                CurrentTheme.IsCurrent = false;

            if (theme.Uri != null)
                ThemeFactory.ChangeTheme(theme.Uri);
            else
                ThemeFactory.ResetTheme();

            CurrentTheme = theme;
            CurrentTheme.IsCurrent = true;

            Settings.Theme = CurrentTheme.Uri;
        }

        private static IList<Theme> themes;
    }
}
