using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public class DataResult
    {

        [JsonProperty("numQueued")]
        public int NumQueued { get; set; }

        [JsonProperty("entries")]
        public IList<Entry> Entries { get; set; }

        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        [JsonProperty("totalTransactionFee")]
        public string TotalTransactionFee { get; set; }

        public override string ToString()
        {
            var result = JsonConvert.SerializeObject(this);
            return result;
        }
    }
}