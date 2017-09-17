using System.Collections.Generic;
using  Newtonsoft.Json;
namespace AfricasTalkingCS
{
    public class B2BData
    {
        public string username { get; set; }
        public string productName { get; set; }
        public string provider { get; set; }
        public string currencyCode { get; set; }
        public decimal amount { get; set; }
        public string transferType { get; set; }
        public string destinationChannel { get; set; }
        public string destinationAccount { get; set; }
        public dynamic metadata { get;  set; }

    }
}