using PayementMVC.Data;
using PayementMVC.Models;
using PayementMVC.Security;
using Polly;
using System.Net;

namespace PayementMVC.Utility
{
    public class GlobalVariable : IGlobalVariable
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<GlobalVariable> _logger;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;


        public GlobalVariable(ILogger<GlobalVariable> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;

            _retryPolicy = PollyPolicy.RetryPolicy(logger);
        }

        const string baseUrl = "https://localhost:7203/api/Admin";


        public async Task<ResponseModel> PostMethod(string url, object Data)
        {
            var response = new ResponseModel();

            try
            {
                var apiresponse = await _httpClient.PostAsJsonAsync(baseUrl + url, Data);
                if (apiresponse.StatusCode == HttpStatusCode.OK)
                {
                    response = await apiresponse.Content.ReadFromJsonAsync<ResponseModel>();
                }
                else if (apiresponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    response.Message = TransactionStatus.Failed.ToString();
                }
                else if (apiresponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    response.Message = TransactionStatus.Pending.ToString();
                }
                else
                {

                    response.Message = TransactionStatus.Pending.ToString();
                }
            }
            catch (Exception ex)//no connection => fail , read timeout => pending
            {
                var retryresp = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsJsonAsync(baseUrl + url, Data));

                if (ex is OperationCanceledException)
                {
                    response.Message = TransactionStatus.Pending.ToString();
                }
                else
                {
                    response.Message = TransactionStatus.Failed.ToString();
                }

            }

            return response;
        }

        public async Task<ResponseModel> GetMethod(string url)
        {
            var response = new ResponseModel();
            var retryresp = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(baseUrl +url));

            var apiresponse = await _httpClient.GetAsync(baseUrl + url);
            if (apiresponse.StatusCode == HttpStatusCode.OK)
            {
                response = await apiresponse.Content.ReadFromJsonAsync<ResponseModel>();
            }
            else
            {
                response.Message = apiresponse.ReasonPhrase;
            }
            return response;
        }
    }
}
