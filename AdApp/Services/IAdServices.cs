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
    public interface IAdServices
    {
        public Task AddingSomeAdToAzure(CloudTable table, AdTable model);
        public Task DeletingSomeAdFromAzue(CloudTable table, string delete);
        public Task<List<AdTable>> GetAll(CloudTable table);
        public Task<AdTable> GetOne(CloudTable table, string id);
        public Task EditSomeAdFromAzure(CloudTable table, AdTable model, string id);
    }
}
