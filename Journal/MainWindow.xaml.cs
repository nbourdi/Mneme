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
        private JournalEntry selectedEntry = null;

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
            selectedEntry = null;
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

            if (selectedEntry != null)
            {
                // Update the existing entry
                selectedEntry.Title = title;
                selectedEntry.Content = content;

                // Refresh the ListBox
                EntriesListBox.Items.Refresh();

                selectedEntry = null; // Clear after updating
            }
            else
            {
                // Create new entry
                var newEntry = new JournalEntry
                {
                    Title = title,
                    Content = content,
                    DateCreated = DateTime.Now
                };

                entries.Add(newEntry);
                EntriesListBox.Items.Add(newEntry);
            }

            SaveEntries(); // Save to file

            TitleTextBox.Text = string.Empty;
            ContentTextBox.Text = string.Empty;
            EntriesListBox.SelectedItem = null;
        }


        private void EntriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntriesListBox.SelectedItem is JournalEntry entry)
            {
                selectedEntry = entry;
                TitleTextBox.Text = entry.Title;
                ContentTextBox.Text = entry.Content;
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EntriesListBox.SelectedItem is JournalEntry entryToDelete)
            {
                var result = MessageBox.Show($"Are you sure you want to delete \"{entryToDelete.Title}\"?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    entries.Remove(entryToDelete);
                    EntriesListBox.Items.Remove(entryToDelete);
                    SaveEntries(); // Save to file

                    // Clear fields
                    TitleTextBox.Text = string.Empty;
                    ContentTextBox.Text = string.Empty;
                    selectedEntry = null;
                }
            }
            else
            {
                MessageBox.Show("Please select an entry to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    public class JournalEntry
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        // Add a parameterless constructor
        public JournalEntry() { }

        // Keep the existing constructor with parameters
        public JournalEntry(string title, string content)
        {
            Title = title;
            Content = content;
            DateCreated = DateTime.Now;
        }
    }
}
