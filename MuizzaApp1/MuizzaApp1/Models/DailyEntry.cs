using System;

namespace MuizzaApp1.Models
{
    public class DailyEntry
    {
        public string Affirmation { get; set; }
        public string Intention { get; set; }
        public string Gratitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 