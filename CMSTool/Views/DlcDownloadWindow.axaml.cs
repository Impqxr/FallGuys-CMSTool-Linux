using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FGCMSTool.Views
{
    public partial class DlcWindow : Window
    {
        class DlcItem
        {
            [JsonProperty("path")]
            public string? Path { get; set; }

            [JsonProperty("base")]
            public string? Base { get; set; }
        }

        class DlcImage
        {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("dlc_item")]
            public DlcItem? DlcItem { get; set; }
        }

        readonly ObservableCollection<string> log = [];
        string? SaveDir;
        public bool isSuceed = false;

        public DlcWindow() => InitializeComponent();

        public DlcWindow(JArray dlcImages, string savePath)
        {
            InitializeComponent();

            LogsControl.ItemsSource = log;

            Loaded += (sender, e) =>
            {
                SaveDir = savePath;

                var images = dlcImages.ToObject<HashSet<DlcImage>>();

                if (images != null)
                    Begin(images);
            };
        }

        async void Begin(HashSet<DlcImage> images)
        {
            int processed = 0;
            int failed = 0;

            if (Directory.Exists(SaveDir))
                Directory.Delete(SaveDir, true);

            Directory.CreateDirectory(SaveDir);

            foreach (var img in images)
            {
                if (img?.DlcItem?.Base == null || img.DlcItem?.Path == null)
                    continue;

                log.Add($"{processed + 1}: Downloading \"{img.Id}\"...");

                try
                {
                    await GetImage($"{img.DlcItem.Base}{img.DlcItem.Path}", img.Id);
                    log[^1] += " [Saved]";
                }
                catch
                {
                    log[^1] += " [Failed]";
                    failed++;
                }

                processed++;

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    DownloadProgress.Value = (int)((double)processed / images.Count * 100);
                    DownloadProgress.ProgressTextFormat = string.Format("{0} / {1} Done - Failed: {2} ({3:0}%)", processed, images.Count, failed, DownloadProgress.Percentage);
                    DownloadProgressLog.Offset = new Vector(0, DownloadProgressLog.Extent.Height);
                });
            }

            log.Add("Download finished. You can close this window now");
            isSuceed = true;
#if RELEASE_WIN_X64 || DEBUG
            SystemSounds.Exclamation.Play();
#endif
        }

        async Task GetImage(string url, string dlcName)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            byte[] data = await response.Content.ReadAsByteArrayAsync();

            await File.WriteAllBytesAsync(Path.Combine(SaveDir, $"{dlcName}.png"), data);
        }
    }
}