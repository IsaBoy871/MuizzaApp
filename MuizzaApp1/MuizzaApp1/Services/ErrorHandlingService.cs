public class ErrorHandlingService
{
    public static string GetUserFriendlyError(Exception ex)
    {
        return ex switch
        {
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.TooManyRequests } 
                => "We're experiencing high demand. Please try again in a few minutes.",
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.Unauthorized } 
                => "Authentication error. Please contact support.",
            TimeoutException 
                => "The request timed out. Please check your internet connection.",
            _ => "An unexpected error occurred. Please try again later."
        };
    }
} 