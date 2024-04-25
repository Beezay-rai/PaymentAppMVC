using System.Net;

namespace PayementMVC.Security
{
    public class Test : ITest
    {

        private readonly HttpClient _httpClient;

        private readonly ILogger<Test> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Test(ILogger<Test> logger, HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }



        const string baseUrl = "https://localhost:7203/api/Admin";


        public async Task<HttpResponseMessage> PostMethod(string url, object Data)
        {
            var response = new HttpResponseMessage();
            try
            {

                response = await _httpClient.PostAsJsonAsync(baseUrl + url, Data);
            }
            catch
            {

            }





            return response;
        }

        public async Task<HttpResponseMessage> GetMethod(string url)
        {
            var response = new HttpResponseMessage();

            try
            {
                var httpclient = _httpClientFactory.CreateClient("myclient");
                var retryresp = await httpclient.GetAsync(baseUrl + url);
                response = await httpclient.GetAsync(baseUrl + url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured:  {ex.Message},exceptionType: {ex.GetType()}");
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
    }
}
