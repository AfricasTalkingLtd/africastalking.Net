using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AfricasTalkingCS
{
    public class Stash
    {
        public Stash(string productName, string currencyCode, decimal amount, Dictionary<string, string> metadata)
        {
            Amount = amount;
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
        }

        [JsonProperty("productName")] public string ProductName { get; set; }

        [JsonProperty("currencyCode")] public string CurrencyCode { get; set; }

        [JsonProperty("amount")] public decimal Amount { get; set; }

        [JsonProperty("metadata")] public Dictionary<string, string> Metadata { get; set; }

        public void AddMetadata(string key, string value)
        {
            Metadata.Add(key, value);
        }
    }
}