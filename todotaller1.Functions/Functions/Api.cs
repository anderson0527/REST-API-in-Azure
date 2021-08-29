using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using todotaller1.Common.Models;
using todotaller1.Common.Responses;
using todotaller1.Functions.Entities;

namespace todotaller1.Functions.Functions{
    public static class Api{
        [FunctionName(nameof(CreateRecordLogin))]
        public static async Task<IActionResult> CreateRecordLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create-recordLogin")] HttpRequest req,
            [Table("recordLogin", Connection = "AzureWebJobsStorage")] CloudTable recordLoginTable, ILogger log){

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RecordLogin recordLogin = JsonConvert.DeserializeObject<RecordLogin>(requestBody);

            if (string.IsNullOrEmpty(recordLogin.IdEmployee.ToString()))
                return new BadRequestObjectResult(new Response{
                    IsSuccess = false,
                    Message = "The request must have a IdEmployee"
                });

            RecordLoginEntity recordLoginEntity = new RecordLoginEntity{
                IdEmployee = recordLogin.IdEmployee,
                LoginExit = recordLogin.LoginExit,
                Type = recordLogin.Type,
                Consolidated = false,
                ETag = "*",
                PartitionKey = "recordLogin",
                RowKey = Guid.NewGuid().ToString(),
            };
            TableOperation addOperation = TableOperation.Insert(recordLoginEntity);
            await recordLoginTable.ExecuteAsync(addOperation);

            string message = "New recordLogin estorage in table";
            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = message,
                Result = recordLoginEntity
            });
        }
        [FunctionName(nameof(UpdateRecordLogin))]
        public static async Task<IActionResult> UpdateRecordLogin(
           [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "update-recordLogin/{id}")] HttpRequest req,
           [Table("recordLogin", Connection = "AzureWebJobsStorage")] CloudTable recordLoginTable, string id, ILogger log){

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RecordLogin recordLogin = JsonConvert.DeserializeObject<RecordLogin>(requestBody);

            TableOperation findOperation = TableOperation.Retrieve<RecordLoginEntity>("recordLogin", id);
            TableResult findResult = await recordLoginTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
                return new BadRequestObjectResult(new Response{
                    IsSuccess = false,
                    Message = "RecordLogin not found."
                });
            RecordLoginEntity recordLoginEntity = (RecordLoginEntity)findResult.Result;
            if (!string.IsNullOrEmpty(recordLogin.IdEmployee.ToString())){
                recordLoginEntity.IdEmployee = recordLogin.IdEmployee;
                recordLoginEntity.LoginExit = recordLogin.LoginExit;
                recordLoginEntity.Type = recordLogin.Type;
            }
            TableOperation addOperation = TableOperation.Replace(recordLoginEntity);
            await recordLoginTable.ExecuteAsync(addOperation);

            string message = $"Todo: {id}, updated in table.";
            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = message,
                Result = recordLoginEntity
            });
        }

        [FunctionName(nameof(GetAllRecordLogins))]
        public static async Task<IActionResult> GetAllRecordLogins(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "all-recordLogins")] HttpRequest req,
            [Table("recordLogin", Connection = "AzureWebJobsStorage")] CloudTable recordLoginTable, ILogger log){

            TableQuery<RecordLoginEntity> query = new TableQuery<RecordLoginEntity>();
            TableQuerySegment<RecordLoginEntity> allRecordLogin = await recordLoginTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all recordLogin.";
            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = message,
                Result = allRecordLogin
            });
        }

        [FunctionName(nameof(GetRecordLoginById))]
        public static IActionResult GetRecordLoginById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-recordLogin/{id}")] HttpRequest req,
            [Table("recordLogin", "recordLogin", "{id}", Connection = "AzureWebJobsStorage")] RecordLoginEntity recordLoginEntity, string id, ILogger log){
            if (recordLoginEntity == null)
                return new BadRequestObjectResult(new Response{
                    IsSuccess = false,
                    Message = "RecordLogin not found."
                });
            string message = $"Retrieved todo {id}.";
            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = message,
                Result = recordLoginEntity
            });
        }

        [FunctionName(nameof(DeleteRecordLogin))]
        public static async Task<IActionResult> DeleteRecordLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete-recordLogin/{id}")] HttpRequest req,
            [Table("recordLogin", "recordLogin", "{id}", Connection = "AzureWebJobsStorage")] RecordLoginEntity recordLoginEntity,
            [Table("recordLogin", Connection = "AzureWebJobsStorage")] CloudTable todoTable, string id, ILogger log){

            if (recordLoginEntity == null)
                return new BadRequestObjectResult(new Response{
                    IsSuccess = false,
                    Message = "Todo not found."
                });
            await todoTable.ExecuteAsync(TableOperation.Delete(recordLoginEntity));
            string message = $"Todo: {recordLoginEntity.RowKey}, deleted.";
            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = message,
                Result = recordLoginEntity
            });
        }
        [FunctionName(nameof(GetAllByDate))]
        public static async Task<IActionResult> GetAllByDate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-recordHour/{DateWorked}")] HttpRequest req,
            [Table("recordHour", Connection = "AzureWebJobsStorage")] CloudTable recordHourTable, DateTime DateWorked, ILogger log){
            
            log.LogInformation($"//////////////////// date {DateWorked} ");
            log.LogInformation($"//////////////////// date {DateWorked.Day-1} ");
            log.LogInformation($"//////////////////// date {DateWorked.AddDays(1)} ");

            string filter = TableQuery.GenerateFilterConditionForDate("DateWorked", QueryComparisons.GreaterThanOrEqual, DateWorked);
            string filter2 = TableQuery.GenerateFilterConditionForDate("DateWorked", QueryComparisons.LessThan, DateWorked.AddDays(1));
            //filter > filter2 ? 
            TableQuery<RecordHourEntity> tableQuery = new TableQuery<RecordHourEntity>().Where(filter2).Where(filter);
            TableQuerySegment<RecordHourEntity> allRecordsHours = await recordHourTable.ExecuteQuerySegmentedAsync(tableQuery, null);

            return new OkObjectResult(new Response{
                IsSuccess = true,
                Message = "Retrieved all allRecordsHours.",
                Result = allRecordsHours
            });
        }
    }
}
