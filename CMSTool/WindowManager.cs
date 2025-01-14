using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;

namespace FGCMSTool
{
    public class WindowManager
    {
        static WindowManager? _instance;

        public static WindowManager Instance
        {
            get => _instance ??= new WindowManager();
        }

        List<Window> ActiveWindows;

        public void Setup()
        {
            Application.Current.PlatformSettings.ColorValuesChanged += (_, _) =>
            {
                ProcessTheme(Application.Current.PlatformSettings.GetColorValues().ThemeVariant);
            };
        }

        void ProcessTheme(PlatformThemeVariant theme)
        {

            for (int i = 0; i < ActiveWindows.Count; i++)
            {
                Window? window = ActiveWindows[i];
                switch (theme)
                {
                    case PlatformThemeVariant.Light:
                        break;
                    case PlatformThemeVariant.Dark:
                        break;
                }
            }
        }

        public void SetupWindow(Window window)
        {
            ActiveWindows ??= [];

            ActiveWindows.Add(window);
        }

        public void RemoveWindow(Window window)
        { 
            ActiveWindows.Remove(window); 
        }
    }
}
