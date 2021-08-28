using System;

namespace todotaller1.Common.Models{
    public class RecordLogin{
        public int IdEmployee { get; set; }
        public DateTime LoginExit { get; set; }
        public string Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
