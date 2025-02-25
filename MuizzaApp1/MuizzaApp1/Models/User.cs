using System.Text.Json.Serialization;

public class User
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long Id { get; set; }
    
    [JsonIgnore]
    public string StringId => Id.ToString();
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("appleUserId")]
    public string AppleUserId { get; set; }
    
    [JsonPropertyName("subscriptionTier")]
    public string SubscriptionTier { get; set; }
    
    [JsonPropertyName("subscriptionEndDate")]
    public DateTime? SubscriptionEndDate { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
} 