using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using AdApp.Model;
using AdApp.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AdApp
{
    public class Delete
    {
        private readonly string _storageAccountManagerConnectionString;
        private readonly string _tableConnector;
        private readonly IAdServices _adServices;

        public Delete(IConfiguration configuration, IAdServices adServices)
        {
            _storageAccountManagerConnectionString = configuration.GetConnectionString("AzureStorageAccountConnectionString");
            _tableConnector = configuration.GetConnectionString("AzureTableName");
            _adServices = adServices;
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = "delete")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //polaczenie z storage account
            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(_storageAccountManagerConnectionString);
            //polaczenie z konkretna tabelka na azurze
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_tableConnector);

            var delete = req.Query["id"];

            await _adServices.DeletingSomeAdFromAzue(table, delete);    

            return null;
        }
    }
}
