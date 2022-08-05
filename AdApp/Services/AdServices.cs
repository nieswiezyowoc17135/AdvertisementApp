using AdApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Services
{
    public class AdServices : IAdServices
    {
        public async Task AddingSomeAdToAzure(CloudTable table, AdTable model)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(model);
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
        }

        public async Task DeletingSomeAdFromAzue(CloudTable table, string id)
        {
            TableOperation retriveOperation = TableOperation.Retrieve<AdTable>(id, id);
            TableResult tableResult = await table.ExecuteAsync(retriveOperation);
            var temporaryVariable = tableResult.Result as AdTable;
            TableOperation deleteOperation = TableOperation.Delete(temporaryVariable);
            await table.ExecuteAsync(deleteOperation);
            TableResult result = await table.ExecuteAsync(deleteOperation);
        }

        public async Task<List<AdTable>> GetAll(CloudTable table)
        {
            var customQuery = new TableQuery<AdTable>();
            var entities = await table.ExecuteQuerySegmentedAsync<AdTable>(customQuery, null);
            var list = entities.Results.ToList();
            return list;
        }

        public async Task<AdTable> GetOne(CloudTable table, string id)
        {
            var customQuery = new TableQuery<AdTable>();
            var entities = await table.ExecuteQuerySegmentedAsync<AdTable>(customQuery, null);
            var elementToDisplay = entities.Results.First(x => x.RowKey == id);
            AdTable elementToPass = new AdTable();
            
            elementToPass.RowKey = id;
            elementToPass.PartitionKey = id;
            elementToPass.Title = elementToDisplay.Title;
            elementToPass.Ad = elementToDisplay.Ad;

            return elementToPass;
        }

        public async Task EditSomeAdFromAzure(CloudTable table, AdTable data ,string id)
        {
            TableOperation retriveOperation = TableOperation.Retrieve<AdTable>(id, id);
            TableResult tableResult = await table.ExecuteAsync(retriveOperation);
            var temporaryVariable = tableResult.Result as AdTable;
            temporaryVariable.Title = data.Title;
            temporaryVariable.Ad = data.Ad;
            temporaryVariable.Type = data.Type;
            TableOperation update = TableOperation.Replace(temporaryVariable);
            await table.ExecuteAsync(update);
        }
    }
}
