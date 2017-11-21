using Newtonsoft.Json;
using System.Collections.Generic;

namespace AfricasTalkingCS
{
    public class CardCheckoutDetails
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("paymentCard")]
        public CardDetails PaymentCard { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("narration")]
        public string Narration { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }
        public void AddMetadata(string key, string value)
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
