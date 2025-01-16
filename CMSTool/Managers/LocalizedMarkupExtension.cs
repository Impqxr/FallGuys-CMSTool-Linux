using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace FGCMSTool.Managers
{
    public class LocalizedMarkupExtension(string key) : MarkupExtension
    {
        public string Key { get; set; } = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => LocalizationManager.LocalizedString(Key);
    }
}
