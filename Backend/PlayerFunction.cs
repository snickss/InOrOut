using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class PlayerFunction
    {
        [FunctionName("playerSave")]
        public async Task<PlayerEntity> PlayerSave([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "player")] PlayerEntity playerEntity)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));

            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Players");
            await table.CreateIfNotExistsAsync();

            var operation = TableOperation.InsertOrMerge(playerEntity);

            var result = await table.ExecuteAsync(operation);

            return result.Result as PlayerEntity;
        }

        [FunctionName("playerGet")]
        public async Task<PlayerEntity> PlayerGet([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "player/{id}")] HttpRequest req, string id)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));

            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Players");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<PlayerEntity>()
                .Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, id));

            var result = table.ExecuteQuery(query);

            return result.FirstOrDefault();
        }

        [FunctionName("playersGet")]
        public async Task<List<PlayerEntity>> PlayersGet([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "player")] HttpRequest req)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));

            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Players");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<PlayerEntity>()
                .Take(null);
                //.Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, id));

            var result = table.ExecuteQuery(query);

            return result.ToList();
        }
    }
}