namespace MuizzaApp1.Models
{
    public class EmotionalResponse
    {
        public string Affirmation { get; set; }
        public string Explanation { get; set; }
        public DateTime CachedAt { get; set; }

        public EmotionalResponse(string affirmation, string explanation)
        {
            Affirmation = affirmation;
            Explanation = explanation;
            CachedAt = DateTime.UtcNow;
        }

        // Parameterless constructor for deserialization
        public EmotionalResponse()
        {
        }
    }
} 