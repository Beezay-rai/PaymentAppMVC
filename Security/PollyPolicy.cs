using Polly;

namespace PayementMVC.Security
{
    public static class PollyPolicy
    {

        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy(ILogger logger)
        {
            var retryPolicy = Policy.Handle<HttpRequestException>().RetryAsync(3, onRetry: (exception, retryCount) =>
            {
                logger.LogWarning($"Retry attempt {retryCount} due to unsuccessful HTTP request: {exception.Message}");
            });

            return retryPolicy.AsAsyncPolicy<HttpResponseMessage>(); // Explicit conversion
        }
    }
}
