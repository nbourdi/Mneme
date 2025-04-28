using System;

namespace Journal
{
    public class JournalEntry
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public override string ToString()
        {
            return $"{DateCreated:yyyy-MM-dd} - {Title}";
        }
    }
}
