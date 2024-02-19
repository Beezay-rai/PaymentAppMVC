using PayementMVC.Models;
using System.Net;

namespace PayementMVC.Utility
{
    public static class GlobalVariable
    {

        static HttpClient _httpclient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        const string baseUrl = "https://localhost:7203/api/Admin";

        public static async Task<ResponseModel> GetMethod(string url)
        {
            var response = new ResponseModel();

            var apiresponse = await _httpclient.GetAsync(baseUrl + url);
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
        public static async Task<ResponseModel> PostMethod(string url, object Data)
        {
            var response = new ResponseModel();

            var apiresponse = await _httpclient.PostAsJsonAsync(baseUrl + url, Data);
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
