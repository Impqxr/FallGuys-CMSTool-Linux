﻿using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using static FGCMSTool.SettingsManager;

namespace FGCMSTool.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                EncryptCombo.SelectedIndex = (int)Settings.SavedSettings.EncryptStrart;
                DecryptCombo.SelectedIndex = (int)Settings.SavedSettings.DecryptStrat;
                XorKey.Text = Settings.SavedSettings.XorKey;
            };
        }

        void SetEncryptStrat(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            Settings.SavedSettings.EncryptStrart = (EncryptStrart)combo.SelectedIndex;

            switch (Settings.SavedSettings.EncryptStrart)
            {
                case EncryptStrart.V1:
                    EncryptCombo_Desc.Text = "Content will be encrypted only via selected XOR Key.";
                    break;
                case EncryptStrart.V2:
                    EncryptCombo_Desc.Text = "Content will be encrypted with both, selected XOR Key and GZIP encoding.";
                    break;
            }
        }

        void SetDecryptStrat(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            Settings.SavedSettings.DecryptStrat = (DecryptStrat)combo.SelectedIndex;

            switch (Settings.SavedSettings.DecryptStrat)
            {
                case DecryptStrat.Default:
                    DecryptCombo_Desc.Text = "Decrypts the content file exactly as it was encrypted.";
                    break;
                case DecryptStrat.Formatting:
                    DecryptCombo_Desc.Text = "Decrypts the content file and formats it with indentation for more readable look.";
                    break;
                case DecryptStrat.Parts:
                    DecryptCombo_Desc.Text = "Decrypts the content file and splits it into parts for easier editing.";
                    break;
            }
        }

        void OnXorChanged(object sender, TextChangedEventArgs e)
        {
            var box = (TextBox)sender;
            Settings.SavedSettings.XorKey = box.Text;
        }

        void ResetXor(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Settings.DefaultSettings.XorKey);
            XorKey.Text = Settings.DefaultSettings.XorKey;
        }

        void SaveSettings(object sender, RoutedEventArgs e)
        {
            Settings.Save();
            this.Close();
        }
    }
}