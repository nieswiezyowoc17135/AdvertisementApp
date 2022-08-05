using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AdApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AdApp.Model;

namespace AdApp
{
    public class Edit
    {
        private readonly string _storageAccountManagerConnectionString;
        private readonly string _tableConnector;
        private readonly IAdServices _adServices;

        public Edit(IConfiguration configuration, IAdServices adServices)
        {
            _storageAccountManagerConnectionString = configuration.GetConnectionString("AzureStorageAccountConnectionString");
            _tableConnector = configuration.GetConnectionString("AzureTableName");
            _adServices = adServices;
        }
        [FunctionName("Edit")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "edit")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //polaczenie z storage account
            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(_storageAccountManagerConnectionString);
            //polaczenie z konkretna tabelka na azurze
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_tableConnector);

            var id = req.Query["id"];
            var Title = req.Query["title"];
            var Ad = req.Query["ad"];
            var Type = req.Query["type"];

            AdTable ad = new AdTable()
            {
                PartitionKey = id,
                RowKey = id,
                Title = Title,
                Ad = Ad,
                Type = Type
            };

            await _adServices.EditSomeAdFromAzure(table, ad, id);
            return null;        
        }
    }
}
