using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public class MobileB2CRecepient
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; private set; }

        public MobileB2CRecepient(string name ,string phoneNumber, string currencyCode, decimal amount)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            CurrencyCode = currencyCode;
            Amount = amount;
            Metadata = new Dictionary<string, string>();
        }

        public void AddMetadata(string key, string value)
        {
            this.Metadata.Add(key,value);
        }

        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}