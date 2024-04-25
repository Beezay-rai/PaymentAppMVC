namespace PayementMVC.Security
{
    public interface ITest
    {
        Task<HttpResponseMessage> PostMethod(string url, object Data);
        Task<HttpResponseMessage> GetMethod(string url);


    }
}
