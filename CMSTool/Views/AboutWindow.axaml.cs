using System;
using System.Reflection;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia;

namespace FGCMSTool.Views
{
    public partial class AboutWindow : Window
    {
        string commit = string.Empty;
        const string Unknown = "Unknown";
        public AboutWindow()
        {
            InitializeComponent();
#if !RELEASE_WIN_X64 || !DEBUG
            MenuTitle.IsVisible = false;
            MainContent.Margin = new Thickness(20, 10, 20, 0);
#endif

            Loaded += (sender, e) =>
            {
                string[]? versionAndRev = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split("+");
                if (versionAndRev.Length >= 2)
                {
                    commit = versionAndRev[1].Split('-')[0];
                    SetAppInfo(versionAndRev[0], commit[..7]);
                }
                else
                    SetAppInfo(Unknown, Unknown);
            };
        }

        private void OpenDiscord(object? sender, RoutedEventArgs e) => OpenURL("https://discord.gg/PEysxvSE3x");
        private void OpenGithubRepo(object? sender, RoutedEventArgs e) => OpenURL("https://github.com/floyzi/FallGuys-CMSTool");
        private void OpenCommit(object? sender, RoutedEventArgs e) => OpenURL($"https://github.com/floyzi/FallGuys-CMSTool/commit/{commit}");

        void OpenURL(string url) => Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });

        void SetAppInfo(string ver, string rev)
        {
            AppVer.Text = string.Format(AppVer.Text, ver);
            AppRev.Text = string.Format(AppRev.Text, rev);
        }
    }
}
