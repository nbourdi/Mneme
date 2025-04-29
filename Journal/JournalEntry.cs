public class JournalEntry
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }

    public JournalEntry(string title, string content)
    {
        Title = title;
        Content = content;
        DateCreated = DateTime.Now;
    }
}
