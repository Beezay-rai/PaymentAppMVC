using PayementMVC.Models;

namespace PayementMVC.Utility
{
    public static class GlobalVariable
    {
        const string baseUrl = "https://localhost:7203/api/Admin";
        static HttpClient _httpclient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };


        public static async Task<ResponseModel> GetMethod(string url)
        {
            var response = await _httpclient.GetAsync(baseUrl + url);
            var result = await response.Content.ReadFromJsonAsync<ResponseModel>();
            return result;
        }
        public static async Task<ResponseModel> PostMethod(string url, object Data)
        {
            var response = await _httpclient.PostAsJsonAsync(baseUrl + url, Data);
            var result = await response.Content.ReadFromJsonAsync<ResponseModel>();
            return result;
        }

    }
}
