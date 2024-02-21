using PayementMVC.Models;

namespace PayementMVC.Utility
{
    public interface IGlobalVariable
    {
        Task<ResponseModel> GetMethod(string url);
        Task<ResponseModel> PostMethod(string url, object Data);
    }
}
