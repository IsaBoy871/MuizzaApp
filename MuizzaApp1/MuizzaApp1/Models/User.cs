public class User
{
    public int Id { get; set; }
    public string AppleUserId { get; set; }
    public string Name { get; set; }
    public string SubscriptionTier { get; set; } = "Free";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 