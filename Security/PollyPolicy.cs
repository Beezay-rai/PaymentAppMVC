using Polly;
using Polly.CircuitBreaker;

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


        #region Circuit Breaker

        private static AsyncCircuitBreakerPolicy _circuitBreaker;


        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakPolicy(ILogger logger)
        {
            _circuitBreaker = Policy.Handle<HttpRequestException>().CircuitBreakerAsync(4, TimeSpan.FromSeconds(5),
               onBreak: (exception, timespan) =>
               {
                   logger.LogError($"Circuit breaked  due to unsuccessful HTTP request:{exception.Message},timespan: {timespan}");
                   Task.Run(() => ResetCircuit(6));
               }, onReset: () =>
               {
                   logger.LogInformation("Circuit Breaker Reset !");


               });
            return _circuitBreaker.AsAsyncPolicy<HttpResponseMessage>();
        }

        public static void ResetCircuit(int delaysec)
        {
            Task.Delay(TimeSpan.FromSeconds(delaysec)).Wait();
            _circuitBreaker.Reset();
        }

        #endregion

    }
}
