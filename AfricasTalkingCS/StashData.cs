using Newtonsoft.Json;
using System.Collections.Generic;

namespace AfricasTalkingCS
{
    public partial class StashData
    {
        [JsonProperty("username")] public string Username { get; set; }

        [JsonProperty("productName")] public string ProductName { get; set; }

        [JsonProperty("currencyCode")] public string CurrencyCode { get; set; }

        [JsonProperty("amount")] public decimal Amount { get; set; }

        [JsonProperty("metadata")] public Dictionary<string, string> Metadata { get; set; }
    }

    public partial class StashData
    {
        public static StashData StashDataJson(string json)
        {
            return JsonConvert.DeserializeObject<StashData>(json, ObjConverter.Settings);
        }
    }
}