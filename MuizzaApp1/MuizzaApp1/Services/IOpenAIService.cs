public interface IOpenAIService
{
    Task<(string affirmation, string explanation)> GenerateResponse(string feeling);
    Task<int> GetRemainingSearches();
    Task<string> GenerateDeepDiveResponse(string feeling, string advisorSystemPrompt);
} 