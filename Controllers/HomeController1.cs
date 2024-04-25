using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq; // Ensure you have Newtonsoft.Json package installed

public class ApiDetail
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class EndpointData
{
    public List<ApiDetail> Parameters { get; set; }
    public List<string> Tags { get; set; }
}

public class MyController : Controller
{
    public IActionResult GenerateHtmlFromJson(string jsonData)
    {
        JObject jsonObject = JObject.Parse(jsonData);
        List<string> tagsWithValue = new List<string>();

        foreach (var endpoint in jsonObject["paths"].Children<JProperty>())
        {
            foreach (var method in endpoint.Value.Children<JProperty>())
            {
                var parameterList = "";
                if (method.Value["parameters"] != null)
                {
                    foreach (var parameter in method.Value["parameters"])
                    {
                        parameterList += $@"
                            <tr>
                                <td>{parameter["name"]}<br/>({parameter["in"]})</td>
                                <td><input class='form-control' placeholder='{parameter["name"]}'/></td>
                            </tr>";
                    }
                }

                string methodClassName = "";
                switch (method.Name.ToUpper())
                {
                    case "GET":
                        methodClassName = "btn btn-success";
                        break;
                    case "POST":
                        methodClassName = "btn btn-primary";
                        break;
                    case "PUT":
                        methodClassName = "btn btn-warning";
                        break;
                    case "DELETE":
                        methodClassName = "btn btn-danger";
                        break;
                }

                string listofApis = $@"
                    <div class='card p-2' style='border:silver dotted 1px;'>
                        <div class='card-header d-flex p-0' style='border:silver solid 1px;'>
                            <button type='button' class='btn' onclick='openCardCollapse(this)'>
                                <span class='{methodClassName}'>{method.Name.ToUpper()}</span>
                                <span class='path text-white'>{endpoint.Name}</span>
                            </button>
                        </div>
                        <div class='collapse'>
                            <div class='card-body'>
                                <table class='table'>
                                    <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Description</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {parameterList}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>";

                string tag = $@"
                    <div class='card'>
                        <div class='card-header'>
                            <div style='display:flex; justify-content:space-between'>
                                <h6>{method.Value["tags"][0]}</h6>
                                <button type='button' class='btn btn-secondary' onclick='openCardCollapse(this)'><i class='fa fa-arrow-down'></i></button>
                            </div>
                        </div>
                        <div class='collapse'>
                            <div class='card-body' id='listofapi'>";

                string api = method.Value["tags"][0] + listofApis;

                if (tagsWithValue.Contains(api))
                {
                    tagsWithValue.Add(api);
                }
                else
                {
                    tagsWithValue.Add(api);
                }
            }
        }

        // Now you have the HTML content generated, you can pass it to the View or return it as needed
        return Json(tagsWithValue);
    }
}
