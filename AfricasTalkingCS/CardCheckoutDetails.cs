using Newtonsoft.Json;
using System.Collections.Generic;

namespace AfricasTalkingCS
{
    public class CardCheckoutDetails
    {
        public string username { get; set; }
        public string productName { get; set; }
        public CardDetails paymentCard { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public decimal amount { get; set; }
        public Dictionary<string, string> metadata { get; set; }
        public void AddMetadata(string key, string value)
        {
            metadata.Add(key, value);
        }
        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
