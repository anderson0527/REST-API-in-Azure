using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using todotaller1.Functions.Entities;

namespace todotaller1.Functions.Functions{
    public static class SheduledFunction{
        [FunctionName("SheduledFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
            [Table("recordLogin", Connection = "AzureWebJobsStorage")] CloudTable recordLoginTable,
            [Table("recordHour", Connection = "AzureWebJobsStorage")] CloudTable recordHourTable, ILogger log){

            string filter = TableQuery.GenerateFilterConditionForBool("Consolidated", QueryComparisons.Equal, false);
            TableQuery<RecordLoginEntity> tableQuery = new TableQuery<RecordLoginEntity>().Where(filter);
            TableQuerySegment<RecordLoginEntity> recordLogins = await recordLoginTable.ExecuteQuerySegmentedAsync(tableQuery, null);
            foreach (RecordLoginEntity recordlogin in recordLogins)
                if (recordlogin.Type == "0")
                    foreach (RecordLoginEntity recordExit in recordLogins)
                        if (recordExit.Type == "1" && recordlogin.IdEmployee.Equals(recordExit.IdEmployee)){
                            int cantMinutes = (recordExit.LoginExit.Hour - recordlogin.LoginExit.Hour) * 60 + recordExit.LoginExit.Minute - recordlogin.LoginExit.Minute;
                            recordlogin.Consolidated = true;
                            recordExit.Consolidated = true;
                            await recordLoginTable.ExecuteAsync(TableOperation.Replace(recordlogin));
                            await recordLoginTable.ExecuteAsync(TableOperation.Replace(recordExit));

                            TableOperation findById = TableOperation.Retrieve<RecordHourEntity>("recordHour", recordlogin.IdEmployee.ToString());
                            TableResult findResult = await recordHourTable.ExecuteAsync(findById);
                            RecordHourEntity recordHour;
                            if (findResult.Result == null)
                                recordHour = new RecordHourEntity {
                                    IdEmployee = recordlogin.IdEmployee,
                                    DateWorked = DateTime.UtcNow,
                                    MinutesWorked = cantMinutes,
                                    ETag = "*",
                                    PartitionKey = "recordHour",
                                    RowKey = recordlogin.IdEmployee.ToString(),
                                };
                            else {
                                recordHour = (RecordHourEntity)findResult.Result;
                                recordHour.MinutesWorked += cantMinutes;
                            }
                            TableOperation operation = TableOperation.InsertOrReplace(recordHour);
                            await recordHourTable.ExecuteAsync(operation);
                        }
        }
    }
}
