using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayementMVC.Interfaces;
using PayementMVC.Models;
using PayementMVC.Utility;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Linq;

namespace PayementMVC.Controllers
{
    public class OpenApiController : Controller
    {
        private readonly IOpenApi _openapi;
        private readonly IGlobalVariable _global;
        private readonly HttpClient _httpClient;


        public OpenApiController(IOpenApi openapi,IGlobalVariable globalVariable, HttpClient httpClient)
        {
            _openapi = openapi;
            _global = globalVariable;
            _httpClient = httpClient;
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
            var a = GenerateViewFromJson(abc);


            return Json(abc);
        }


        public void CheckNested(JObject root, JObject obj, ref Dictionary<string, JObject> myDict)
        {

            if (obj["$ref"] != null)
            {
                var a = obj["$ref"].ToString();
                var splitted = a.Replace("#/", "").Split("/");
                var path = string.Join(".", splitted);
                var modelname2 = path.Substring(path.LastIndexOf(".") + 1);
                JObject jToken = root.SelectToken(path) as JObject;
                myDict.Add(modelname2, jToken);
                CheckNested(root, jToken, ref myDict);
            }

        }

        public List<SchemaModel> GenerateViewFromJson(string json)
        {
            List<SchemaModel> myschemaList = new List<SchemaModel>();
            var root = JsonConvert.DeserializeObject<JObject>(json);
            var schemas = root.SelectToken("components.schemas") as JObject;
            foreach (var model in schemas.Properties())
            {
                SchemaModel myschema = new SchemaModel();
                myschema.ModelName = model.Name;

                var modelProperties = (JObject)model.Value;
                foreach (var property in modelProperties.Properties())
                {
                    if (property.Name == "additionalProperties")
                    {
                        continue;
                    }

                    if (property.Name == "properties")
                    {

                        var nestedObj = (JObject)property.Value;
                        foreach (var property2 in nestedObj.Properties())
                        {
                            var testignval = JObject.Parse(property2.Value.ToString());
                            var mydict = new Dictionary<string, JObject>();
                            CheckNested(root, testignval, ref mydict);
                            if (mydict.Count > 0)
                            {
                                myschema.haha.Add(property2.Name, mydict);
                            }
                            else
                            {
                                myschema.NestedPropertiesList.Add(property2.Name, testignval);

                            }


                        }
                    }
                    else
                    {
                        switch (property.Type)
                        {
                            case JTokenType.Object:
                            case JTokenType.Array:
                                break;

                            case JTokenType.Property:
                                myschema.PropertiesList.Add(property.Name, property.Value.ToString());
                                break;
                        }
                    }






                }
                myschemaList.Add(myschema);
            }
            return myschemaList;

        }



        public async Task<IActionResult> PartialViewForSchema(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var abc = await response.Content.ReadAsStringAsync();
            var a = GenerateViewFromJson(abc);
            return PartialView("_SchemaView", a);

        }

        public List<JObject> GetListOfNestedJObject(JObject obj)
        {
            var JObjects = new List<JObject>();

            foreach (var property in obj.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    var nestedJObjects = GetListOfNestedJObject((JObject)property.Value);

                    foreach (var nestedJObject in nestedJObjects)
                    {
                        JObjects.Add(nestedJObject);
                    }


                }

            }

            return JObjects;
        }
        public class SchemaModel
        {
            public string ModelName { get; set; }
            public Dictionary<string, string> PropertiesList { get; set; } = new Dictionary<string, string>();
            public Dictionary<string, JObject> NestedPropertiesList { get; set; } = new Dictionary<string, JObject>();

            public Dictionary<string, Dictionary<string, JObject>> haha = new Dictionary<string, Dictionary<string, JObject>>();



        }




        public async Task<JsonResult> ExecuteApi(string url,string method, string mediaType)
        {
            HttpMethod httpMethod =new  HttpMethod(method);
            
           
            var request = new HttpRequestMessage(httpMethod, url);
            try
            {
                var responseMessage = await _httpClient.SendAsync(request);

                var contentType = responseMessage.Content.Headers.ContentType.MediaType ?? "";

                var values = await responseMessage.Content.ReadAsStringAsync();


                return Json(new { StatusCode = responseMessage.StatusCode, ContentType = contentType, ResponseValue = values, ServerResponse = responseMessage });

            }
            catch (Exception ez)
            {
                return Json(new { StatusCode = 500, ResponseValue = ez.Message, ServerResponse= ez });
            }
            
           
        }

        public IActionResult testingConfigure(int id)
        {
            var data = _openapi.GetById(id);
            return View(data);
        }






        public IActionResult ParameterPartial() => PartialView("_Parameter");
        public IActionResult PathPartial() => PartialView("_Path");
        public IActionResult ResponsePartial() => PartialView("_Response");
    }


}
