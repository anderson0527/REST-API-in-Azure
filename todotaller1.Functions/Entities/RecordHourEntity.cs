using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace todotaller1.Functions.Entities{
    public class RecordHourEntity : TableEntity{
        public int IdEmployee { get; set; }
        public DateTime DateWorked { get; set; }
        public int MinutesWorked { get; set; }
    }
}
