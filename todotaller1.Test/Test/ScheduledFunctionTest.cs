using System;
using todotaller1.Functions.Functions;
using todoanderson.Test.Helpers;
using Xunit;

namespace todotaller1.Test.Test{
    public class ScheduledFunctionTest{
        [Fact]
        public void ScheduledFunction_Should_Log_Message(){
            MockCloudTableRecordLogin mockRecordLogin = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            MockCloudTableRecordLogin mockRecordHour = new MockCloudTableRecordLogin(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreatedLogger(LoggerTypes.List);

            SheduledFunction.Run(null, mockRecordLogin, mockRecordHour, logger);
            string message = logger.Logs[0];

            Assert.Contains("Time worked function", message);
        }
    }
}
