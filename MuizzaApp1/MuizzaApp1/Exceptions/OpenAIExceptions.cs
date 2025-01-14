using System;

namespace MuizzaApp1.Exceptions
{
    public class OpenAIException : Exception
    {
        public OpenAIException(string message) : base(message) { }
        public OpenAIException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class RateLimitExceededException : OpenAIException
    {
        public RateLimitExceededException(string message) : base(message) { }
    }

    public class UnauthorizedException : OpenAIException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
} 