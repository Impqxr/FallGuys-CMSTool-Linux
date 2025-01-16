using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Newtonsoft.Json;

namespace FGCMSTool.Managers
{
    public class LocalizationManager
    {
        static Dictionary<string, string>? LangEntries;
        const string linkDef = @"\{ref:(.*?)\}";

        public static void Setup(string basePath)
        {
            var path = Path.Combine(basePath, "Assets", "Locale", "en.json");
            LangEntries = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
        }

        public static string LocalizedString(string key, object[]? format = null)
        {
            if (LangEntries == null)
                return $"NULL: {key}";

            LangEntries.TryGetValue(key, out var value);

            if (value == null)
                return $"MISSING: {key}";

            string result = value;

            foreach (Match match in Regex.Matches(result, linkDef))
            {
                var refKey = match.Groups[1].Value;
                LangEntries.TryGetValue(refKey, out var value_2);

                if (value_2 != null)
                    result = result.Replace(match.Value, value_2);
                else
                    result = result.Replace(match.Value, $"MISSING: {refKey}");
            }


            if (format != null)
            {
                int waitingForFormat = Regex.Matches(result, @"\{\d+\}").Count;
                if (format.Length == waitingForFormat)
                    result = string.Format(result, format);
            }
            return result;
        }
    }
}
