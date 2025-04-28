using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Journal
{
    public partial class MainWindow : Window
    {
        private List<JournalEntry> entries = new List<JournalEntry>();
        private readonly string saveFilePath = "journal_entries.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadEntries();
        }

        private void NewEntryButton_Click(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Text = string.Empty;
            ContentTextBox.Text = string.Empty;
            EntriesListBox.SelectedItem = null;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string content = ContentTextBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Please fill in both title and content before saving.", "Incomplete Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEntry = new JournalEntry
            {
                Title = title,
                Content = content,
                DateCreated = DateTime.Now
            };

            entries.Add(newEntry);
            EntriesListBox.Items.Add(newEntry);

            SaveEntries(); // Save to file
            MessageBox.Show("Entry saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            TitleTextBox.Text = string.Empty;
            ContentTextBox.Text = string.Empty;
        }

        private void EntriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntriesListBox.SelectedItem is JournalEntry selectedEntry)
            {
                TitleTextBox.Text = selectedEntry.Title;
                ContentTextBox.Text = selectedEntry.Content;
            }
        }

        private void SaveEntries()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(entries, options);
            File.WriteAllText(saveFilePath, json);
        }

        private void LoadEntries()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                entries = JsonSerializer.Deserialize<List<JournalEntry>>(json) ?? new List<JournalEntry>();
                foreach (var entry in entries)
                {
                    EntriesListBox.Items.Add(entry);
                }
            }
        }
    }
}
