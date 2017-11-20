using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricasTalkingCS
{
    class BankTransferDetails
    {
        [JsonProperty("bankAccount")]
        public List<BankAccountDetails> BankAccount { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("narration")]
        public string Narration { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string,string>  Metadata { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        public BankTransferDetails()
        {
            BankAccount = new List<BankAccountDetails>();
        }
        public void AddMetadata (string key, string value)
        {
            Metadata.Add(key, value);
        }
        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
        
    }
}
