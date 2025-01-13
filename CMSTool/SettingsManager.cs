using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FGCMSTool
{
    public class SettingsManager
    {
        public static SettingsManager? Settings { get; set; }

        public enum DecryptStrat
        {
            Default,
            Formatting,
            Parts
        }

        public enum EncryptStrart
        {
            V1,
            V2,
        }


        public class SettingsJson
        {
            public string? XorKey;
            public DecryptStrat DecryptStrat;
            public EncryptStrart EncryptStrart;
        }

        public SettingsJson? SavedSettings;
        readonly SettingsJson DefaultSettings = new()
        {
            XorKey = "a#!sC0,.",
            DecryptStrat = DecryptStrat.Default,
            EncryptStrart = EncryptStrart.V2
        };

        string? ConfigPath;

        public void Load(string baseDir)
        {
            ConfigPath = Path.Combine(baseDir, "config.json");

            try
            {
                if (!File.Exists(ConfigPath))
                {
                    SavedSettings = DefaultSettings;
                }
                else
                {
                    SavedSettings = JsonConvert.DeserializeObject<SettingsJson>(File.ReadAllText(ConfigPath));
                }
            }
            catch
            {
                SavedSettings = DefaultSettings;
            }
        }

        public void Save() => File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(SavedSettings));
    }
}
