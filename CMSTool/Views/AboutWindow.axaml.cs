using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FGCMSTool.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                AppVer.Text = string.Format(AppVer.Text, FileVersionInfo.GetVersionInfo(Environment.ProcessPath).FileVersion);
            };
        }

        private void OpenDiscord(object? sender, RoutedEventArgs e) => OpenURL("https://discord.gg/PEysxvSE3x");
        private void OpenGithubRepo(object? sender, RoutedEventArgs e) => OpenURL("https://github.com/floyzi");

        void OpenURL(string url) => Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}