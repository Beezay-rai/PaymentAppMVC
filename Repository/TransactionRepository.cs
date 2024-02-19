using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using PayementMVC.Data;
using PayementMVC.Interfaces;
using PayementMVC.Models;

namespace PayementMVC.Repository
{
    public class TransactionRepository : ITransaction
    {
        private readonly PaymentDbContext _con;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(PaymentDbContext con, IMapper mapper, ILogger<TransactionRepository> logger)
        {
            _con = con;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<ResponseModel> CheckStatus(string trackingId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> CreateNewBalance(BalanceViewModel model)
        {
            var response = new ResponseModel();
            try
            {
                var data = _mapper.Map<Balance>(model);
                await _con.Balance.AddAsync(data);
                response.Status = true;
                response.Message = "Created Successfully !";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error Occured";
                _logger.LogError($"Error occured in Transaction Repo,{ex},Datetime:{DateTime.UtcNow}");

            }


            return response;

        }

        public async Task<ResponseModel> DeposiAmount(TransactionViewModel model)
        {
            var response = new ResponseModel();
            try
            {
                var data = _mapper.Map<Transaction>(model);
                data.Status = TransactionStatus.Success.ToString();
                await _con.Transaction.AddAsync(data);
                await _con.SaveChangesAsync();
                response.Status = true;
                response.Message = "Deposited Successfully";


            }

            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Failed !";
                _logger.LogError($"Error occured in Transaction Repo,{ex},Datetime:{DateTime.UtcNow}");

            }
            return response;

        }

        public async Task<List<TransactionViewModel>> GetAllTransaction()
        {

            var data = await _con.Transaction.ToListAsync();
            var a = _mapper.Map<List<TransactionViewModel>>(data);
            return a;

        }

        public async Task<ResponseModel> SetTransactionStatus(string trackingid,string transactionStatus)
        {
            var response = new ResponseModel();
            try
            {
                var data = await _con.Transaction.Where(x => x.TrackingId == trackingid).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.Status = transactionStatus.ToString();
                    _con.Entry(data).State = EntityState.Modified;
                    await _con.SaveChangesAsync();
                }

            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = "Error Occured !";

            }
            return response;
        }
    }
}
