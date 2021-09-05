using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace todotaller1.Functions.Entities{
    public class RecordLoginEntity : TableEntity{
        public int IdEmployee { get; set; }
        public DateTime LoginExit { get; set; }
        public string Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
