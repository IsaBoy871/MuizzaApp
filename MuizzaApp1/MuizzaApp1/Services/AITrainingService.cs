using Microsoft.Extensions.Logging;
using MuizzaApp1.Services;

public class AITrainingService
{
    private readonly IOpenAIService _openAIService;
    private readonly ResponseCacheService _cacheService;
    private readonly ILogger<AITrainingService> _logger;

    public AITrainingService(
        IOpenAIService openAIService,
        ResponseCacheService cacheService,
        ILogger<AITrainingService> logger)
    {
        _openAIService = openAIService;
        _cacheService = cacheService;
        _logger = logger;
    }

    // For testing during development, using common scenarios
    private readonly Dictionary<string, List<string>> _testTrainingData = new()
    {
        // Basic Emotions
        { "Common Emotions", new List<string> 
            {
                "anxious",
                "stressed",
                "overwhelmed",
                "sad",
                "lonely"
            }
        },

        // Common Situations
        { "Life Situations", new List<string>
            {
                "struggling to get over my ex",
                "feeling like I'm not good enough",
                "can't sleep at night",
                "hate my job",
                "fighting with my parents"
            }
        },

        // Mental Health
        { "Mental Health", new List<string>
            {
                "having panic attacks",
                "feeling depressed",
                "can't stop overthinking",
                "feeling burnt out",
                "having dark thoughts"
            }
        }
    };

    // ... rest of the service code ...

    // Test training method
    public async Task RunTestTraining()
    {
        int totalProcessed = 0;
        int totalPhrases = _testTrainingData.Sum(category => category.Value.Count);

        foreach (var category in _testTrainingData)
        {
            _logger.LogInformation($"Processing test category: {category.Key}");

            foreach (var phrase in category.Value)
            {
                try
                {
                    var cached = await _cacheService.GetCachedResponse(phrase);
                    if (cached == null)
                    {
                        var response = await _openAIService.GenerateResponse(phrase);
                        await _cacheService.CacheResponse(
                            phrase,
                            response.affirmation,
                            response.explanation
                        );
                        await Task.Delay(1000); // Rate limiting
                    }

                    totalProcessed++;
                    _logger.LogInformation($"Test training progress: {totalProcessed}/{totalPhrases}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing test phrase: {phrase}");
                }
            }
        }

        _logger.LogInformation("Test training completed!");
    }
} 