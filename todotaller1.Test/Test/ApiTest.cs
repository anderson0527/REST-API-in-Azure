using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using todoanderson.Test.Helpers;
using todotaller1.Common.Models;
using todotaller1.Functions.Entities;
using todotaller1.Functions.Functions;
using todotaller1.Test.Helpers;
using Xunit;

namespace todotaller1.Test.Test{
    public class ApiTest{
        private readonly ILogger logger = TestFactory.CreatedLogger();
        [Fact]
        public async void CreateRecordLogin_Should_Return_200(){
            MockCloudTableRecordLogin mockRecordLogin = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            RecordLogin recordLoginRequest = TestFactory.MockRecordLogin();
            DefaultHttpRequest request = TestFactory.CreatedHttpRequest(recordLoginRequest);

            IActionResult response = await Api.CreateRecordLogin(request, mockRecordLogin, logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public async void UpDateRecordLogin_Should_Return_200(){
            MockCloudTableRecordLogin mockRecordLogin = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            RecordLogin recordLoginRequest = TestFactory.MockRecordLogin();
            DefaultHttpRequest request = TestFactory.UpdateHttpRequest(recordLoginRequest);

            var response = await Api.UpdateRecordLogin(request, mockRecordLogin, recordLoginRequest.IdEmployee.ToString(), logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public async void DeleteRecordLogin_Should_Return_200(){
            MockCloudTableRecordLogin mockRecordLogin = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            RecordLoginEntity recordLoginRequest = TestFactory.MockRecordLoginEntity();
            HttpRequest request = TestFactory.DeleteHttpRequest(recordLoginRequest.RowKey, recordLoginRequest);

            IActionResult response = await Api.DeleteRecordLogin(request, recordLoginRequest, mockRecordLogin, recordLoginRequest.IdEmployee.ToString(), logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public void GetRecordLoginById_Should_Return_200(){
            RecordLoginEntity recordLoginRequest = TestFactory.MockRecordLoginEntity();
            HttpRequest request = TestFactory.GetByIdHttpRequest(recordLoginRequest.IdEmployee.ToString());

            IActionResult response = Api.GetRecordLoginById(request, recordLoginRequest, recordLoginRequest.IdEmployee.ToString(), logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public async void GetAllRecordLogins_Should_Return_200(){
            MockCloudTableRecordLogin mockRecordLogin = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.GetAllHttpRequest();

            IActionResult response = await Api.GetAllRecordLogins(request, mockRecordLogin, logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        [Fact]
        public async void GetAllByDate_Should_Return_200(){
            MockCloudTableRecordHour mockRecordHour = new MockCloudTableRecordHour(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.GetAllHttpRequest();

            IActionResult response = await Api.GetAllByDate(request, mockRecordHour, new DateTime(2021, 08, 29), logger);

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
