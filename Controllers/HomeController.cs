using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PayementMVC.Interfaces;
using PayementMVC.Models;
using PayementMVC.Utility;
using System.Diagnostics;
using System.Reflection;

namespace PayementMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        IMemoryCache _memoryCache;
        private readonly ITransaction _transaction;
        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, ITransaction transaction)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _transaction = transaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            model.RequestId  = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(model);
        }


        //

        public IActionResult InsertBalance()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertBalance(BalanceViewModel model)
        {
            try
            {
                var mvcresponse = await _transaction.CreateNewBalance(model);
                if (mvcresponse.Status)
                {
                    var response = await GlobalVariable.PostMethod("/Transaction/CreateNewBalance", model);
                    if (response.Status)
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch
            {

            }
            return View(model);


        }

        public IActionResult Deposit()
        {
            return View(new TransactionViewModel() { TrackingId = DateTime.UtcNow.ToString("dd-mm-ss") });
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(TransactionViewModel model)
        {
            try
            {
                var mvcresponse = await _transaction.DeposiAmount(model);
                if (mvcresponse.Status)
                {
                    var response = await GlobalVariable.PostMethod("/Transaction/DepositAmount", model);
                    if (response!= null && response.Status)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        await _transaction.SetTransactionStatus(model.TrackingId, "Pending");
                        ErrorViewModel errorViewModel = new ErrorViewModel()
                        {
                            ErrorMessage = response.Message
                        };
                        return RedirectToAction("Error", errorViewModel);
                    }
                }


            }
            catch (Exception ex)
            {
                await _transaction.SetTransactionStatus(model.TrackingId, "Pending");
                _logger.LogInformation($"Error occured {ex.Message}, DateTime : {DateTime.UtcNow}");
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public async Task<IActionResult> GetAllTransaction()
        {
            var data = await _transaction.GetAllTransaction();
            return View(data);
        }

        public async Task<ResponseModel> CheckStatus(string trackingId)
        {
            var response = new ResponseModel();
            try
            {
                response = await GlobalVariable.PostMethod("/Transaction/CheckStatus", trackingId);
                if (response != null )
                {
                    await _transaction.SetTransactionStatus(trackingId, response.Data.ToString());

                }



            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;

        }
    }
}
