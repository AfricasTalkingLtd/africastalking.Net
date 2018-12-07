namespace AfricasTalkingCS
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Wallet 
    {
        public Wallet(string productName, int targetProductCode, string currencyCode, decimal amount, Dictionary<string, string> metadata)
        {
            this.Amount = amount;
            this.TargetProductCode = targetProductCode;
            this.ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
        }

        [JsonProperty("targetProductCode")]
        public int TargetProductCode {get; set;}

        [JsonProperty("productName")]
        public string ProductName {get; set;}

        [JsonProperty("currencyCode")]
        public string CurrencyCode {get; set;}

        [JsonProperty("amount")]
        public decimal Amount {get; set;}

        [JsonProperty("metadata")]
        public Dictionary <string, string> Metadata {get; set;}

        public void AddMetadata(string key, string value)
        {
            this.Metadata.Add(key, value);
        }
    }
}