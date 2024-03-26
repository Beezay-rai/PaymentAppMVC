using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PayementMVC.Interfaces;
using PayementMVC.Models;
using System.Xml;

namespace PayementMVC.Controllers
{
    public class OpenApiController : Controller
    {
        private readonly IOpenApi _openapi;

        public OpenApiController(IOpenApi openapi)
        {
            _openapi = openapi;
        }

        public IActionResult Index()
        {
            var data = _openapi.GetAll();
            return View(data);
        }
        [HttpPost]
        public IActionResult Create(OpenApiModel model)
        {
            _openapi.Create(model);


            return RedirectToAction("Index");
        }
        public IActionResult Configure(int id)
        {
            var data = _openapi.GetById(id);
            return View(data);
        }


        public async Task<IActionResult> GetUrlJson(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var abc = await response.Content.ReadAsStringAsync();
           
            return Json(abc);
        }

        public IActionResult ParameterPartial() => PartialView("_Parameter");
        public IActionResult PathPartial() => PartialView("_Path");
        public IActionResult ResponsePartial() => PartialView("_Response");
    }
    public class testobject
    {
        public object components { get; set; }
        public object info { get; set; }
        public object openapi { get; set; }
        public List<object> paths { get; set; }
    }

}
