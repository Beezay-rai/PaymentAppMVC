using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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
        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult InsertBalance()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertBalance(BalanceModel model)
        {
            try
            {
                var response = await GlobalVariable.PostMethod("/Transaction/CreateNewBalance", model);
                if (response.Status)
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {

            }
            return View(model);


        }

        public IActionResult Deposit()
        {
            return View(new TransactionModel() { TrackingId = Guid.NewGuid().ToString() });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(TransactionModel model)
        {
            try
            {
                var response = await GlobalVariable.PostMethod("/Transaction/DepositAmount", model);
                if (response.Status)
                {
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Error occured {ex.Message}, DateTime : {DateTime.UtcNow}");
                TempData["msg"] = "Status on Pending";
                TransactionGETModel abc = new TransactionGETModel()
                {
                    Amount = model.Amount,
                    Status = "Pending",
                    Username = model.Username,
                    TrackingId = model.TrackingId,
                };
                _memoryCache.CreateEntry("pendingtransaction");
                _memoryCache.Set("pendingtransaction", abc, DateTime.UtcNow.AddDays(1));
                return RedirectToAction("GetAllTransaction");
            }

            return View(model);
        }

        public async Task<IActionResult> GetAllTransaction()
        {

            var response = await GlobalVariable.GetMethod("/Transaction/GetAllTransaction");
            if (response.Status)
            {
                var data = JsonConvert.DeserializeObject<List<TransactionGETModel>>(response.Data.ToString());

                var newList = data;
                var abc = (TransactionGETModel)_memoryCache.Get("pendingtransaction");


                if (abc != null && data != null)
                {
                    if (data.Count() > 0)
                    {
                        foreach (var item in data.ToList())
                        {

                            if(item.TrackingId == abc.TrackingId)
                            {
                                continue;
                            }
                            else
                            {
                                if (newList.Any(x=>x.TrackingId == abc.TrackingId))
                                {
                                    continue;
                                }
                                else
                                {

                                newList.Add(abc);
                                }
                            }

                        }

                    }
                    else
                    {
                        newList.Add(abc);
                    }

                }




                return View(newList);
            }

            return View();
        }


        public IActionResult CheckStatus()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckStatus(string TrackingId)
        {
            ViewBag.Msg = "";
            try
            {
                var abc = (TransactionGETModel)_memoryCache.Get("pendingtransaction");

                var response = await GlobalVariable.PostMethod("/Transaction/CheckStatus", TrackingId.ToString());
                if (response != null)
                {

                    ViewBag.Msg = response.Message.ToString();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Msg = "System Failure";
            }
            return View("CheckStatus");

        }
    }
}
