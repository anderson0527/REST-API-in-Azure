using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using todotaller1.Common.Models;
using todotaller1.Functions.Entities;

namespace todoanderson.Test.Helpers{
    public class TestFactory{
        public static RecordLoginEntity MockRecordLoginEntity(){
            return new RecordLoginEntity{
                IdEmployee = 0,
                LoginExit = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                Type = "0",
                Consolidated = false,
                ETag = "*",
                PartitionKey = "recordLogin",
                RowKey = "5c8b2974-7e04-423e-86ea-585d49a51055",
            };
        }
        public static RecordLogin MockRecordLogin(){
            return new RecordLogin{
                IdEmployee = 0,
                LoginExit = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                Type = "0",
                Consolidated = false
            };
        }
        public static RecordHourEntity MockRecordHourEntity(){
            return new RecordHourEntity{
                IdEmployee = 0,
                DateWorked = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                MinutesWorked = 50,
                ETag = "*",
                PartitionKey = "recordHour",
                RowKey = "5c8b2974-7e04-423e-86ea-585d49a51055",
            };
        }
        public static RecordHour MockRecordHour(){
            return new RecordHour{
                IdEmployee = 0,
                DateWorked = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                MinutesWorked = 50
            };
        }
        public static DefaultHttpRequest CreatedHttpRequest(RecordLogin recordLogin){
            string request = JsonConvert.SerializeObject(recordLogin);
            return new DefaultHttpRequest(new DefaultHttpContext()){
                Body = GenerateStreamFromString(request)
            };
        }
        public static DefaultHttpRequest UpdateHttpRequest(RecordLogin recordLogin){
            string request = JsonConvert.SerializeObject(recordLogin);
            return new DefaultHttpRequest( new DefaultHttpContext() ){
                Body = GenerateStreamFromString(request),
                Path = $"/{recordLogin.IdEmployee}"
            };
        }
        public static DefaultHttpRequest DeleteHttpRequest(string Row, RecordLoginEntity TimeRequest){
            string request = JsonConvert.SerializeObject(TimeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext()){
                Body = GenerateStreamFromString(request),
                Path = $"/{Row}"
            };
        }
        public static DefaultHttpRequest GetByIdHttpRequest(string id){
            return new DefaultHttpRequest(new DefaultHttpContext()){
                Path = $"/{id}"
            };
        }
        public static DefaultHttpRequest GetAllHttpRequest(){
            return new DefaultHttpRequest(new DefaultHttpContext());
        }
        public static Stream GenerateStreamFromString(string stringToConvert){
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static ILogger CreatedLogger(LoggerTypes type = LoggerTypes.Null){
            ILogger logger;
            if (type == LoggerTypes.List)
                logger = new ListLogger();
            else 
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            return logger;
        }
        public static List<RecordLoginEntity> MockListRecordsLogins(){
            List<RecordLoginEntity> listRecordsLogins = new List<RecordLoginEntity>();
            RecordLoginEntity recordLogin = new RecordLoginEntity{
                IdEmployee = 0,
                LoginExit = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                Type = "0",
                Consolidated = false,
                ETag = "*",
                PartitionKey = "recordLogin",
                RowKey = "5c8b2974-7e04-423e-86ea-585d49a51055",
            };
            listRecordsLogins.Add(recordLogin);
            return listRecordsLogins;
        }
        public static List<RecordHourEntity> MockListRecordsHours(){
            List<RecordHourEntity> listRecordsHours = new List<RecordHourEntity>();
            RecordHourEntity recordHour = new RecordHourEntity{
                IdEmployee = 0,
                DateWorked = new DateTime(2021, 08, 29, 3, 30, 00, 00),
                MinutesWorked = 50,
                ETag = "*",
                PartitionKey = "recordLogin",
                RowKey = "5c8b2974-7e04-423e-86ea-585d49a51055",
            };
            listRecordsHours.Add(recordHour);
            return listRecordsHours;
        }
    }
}
