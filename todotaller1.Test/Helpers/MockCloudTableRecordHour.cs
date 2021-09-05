using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using todoanderson.Test.Helpers;

namespace todotaller1.Test.Helpers{
    public class MockCloudTableRecordHour : CloudTable{
        public MockCloudTableRecordHour(Uri tableAddress) : base(tableAddress) { }

        public MockCloudTableRecordHour(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials) { }

        public MockCloudTableRecordHour(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials) { }
        public override async Task<TableResult> ExecuteAsync(TableOperation operation){
            return await Task.FromResult(new TableResult{
                HttpStatusCode = 200,
                Result = TestFactory.MockRecordHourEntity()
            });
        }
        public override async Task<TableQuerySegment<RecordHourEntity>> ExecuteQuerySegmentedAsync<RecordHourEntity>(TableQuery<RecordHourEntity> allRecordsHours, TableContinuationToken token){
            ConstructorInfo builder = typeof(TableQuerySegment<RecordHourEntity>).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(x => x.GetParameters().Count() == 1);
            return await Task.FromResult(builder.Invoke(new object[] { TestFactory.MockListRecordsHours() }) as TableQuerySegment<RecordHourEntity>);
        }
    }
}
