using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Functions.DI.Interfaces;

namespace Azure.Functions.DI
{
    public class SimpleHttpFunction
    {
        private readonly ISimpleManager _simpleManager;

        public SimpleHttpFunction(ISimpleManager simpleManager)
        {
            _simpleManager = simpleManager;
        }

        [FunctionName(nameof(SimpleHttpFunction))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "simplehttpfunction")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("SimpleHttpFunction: C# HTTP trigger function processed a request.");

                string name = req.Query["name"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;

                return name != null
                    ? (ActionResult)new OkObjectResult($"Hello, {_simpleManager.ToUpperCase(name)}.")
                    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new BadRequestObjectResult($"Error in SimpleHttpFunction: {ex.Message}");
            }
        }
    }
}