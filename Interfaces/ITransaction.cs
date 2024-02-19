using PayementMVC.Data;
using PayementMVC.Models;

namespace PayementMVC.Interfaces
{
    public interface ITransaction
    {
        Task<ResponseModel> DeposiAmount(TransactionViewModel model);
        Task<ResponseModel> CheckStatus(string trackingId);
        Task<ResponseModel> CreateNewBalance(BalanceViewModel model);
        Task<List<TransactionViewModel>> GetAllTransaction();

        Task<ResponseModel> SetTransactionStatus(string transactionid, string transactionStatus);

    }
}
