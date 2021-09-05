using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace todoanderson.Test.Helpers{
    public class MockCloudTableRecordLogin : CloudTable{
        public MockCloudTableRecordLogin(Uri tableAddress) : base(tableAddress){}

        public MockCloudTableRecordLogin(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials){}

        public MockCloudTableRecordLogin(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials){}
        public override async Task<TableResult> ExecuteAsync(TableOperation operation){
            return await Task.FromResult(new TableResult{
                HttpStatusCode = 200,
                Result = TestFactory.MockRecordLoginEntity()
            });
        }
    }
}
