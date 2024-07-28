using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace sd2990_azure_serverless
{
    public class UpdateProductQuantity
    {
        private static readonly ILogger<UpdateProductQuantity> _logger;

        private static readonly string endpoint = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
        private static readonly string key = Environment.GetEnvironmentVariable("COSMOS_DB_KEY");
        private static readonly CosmosClient client = new CosmosClient(endpoint, key);
        private static readonly Database database = client.GetDatabase("Products");
        private static readonly Container container = database.GetContainer("YourContainerName");

        [Function("UpdateProductQuantity")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string productId = data?.productId;
            int newQuantity = data?.newQuantity;

            try
            {
                ItemResponse<dynamic> response = await container.ReadItemAsync<dynamic>(productId, new PartitionKey(productId));
                dynamic document = response.Resource;
                document.Quantity = newQuantity;

                await container.ReplaceItemAsync(document, productId, new PartitionKey(productId));

                log.LogInformation("SD2990 - Azure Serverless - C# HTTP trigger function processed a request.");
                return new OkObjectResult($"Product updated: {productId}");
            }
            catch (CosmosException ex)
            {
                log.LogError($"SD2990 - CosmosDB error: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            
        }
    }
}
