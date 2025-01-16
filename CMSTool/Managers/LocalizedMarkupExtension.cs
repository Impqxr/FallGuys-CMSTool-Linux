using System;
using Avalonia.Markup.Xaml;

namespace FGCMSTool.Managers
{
    public class LocalizedMarkupExtension(string key) : MarkupExtension
    {
        public string Key { get; set; } = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => LocalizationManager.LocalizedString(Key);
    }
}
