using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public partial class BankTransfer
    {
        [JsonProperty("productName")] public string ProductName { get; set; }

        [JsonProperty("recipients")] public List<BankTransferRecipients> Recipients { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }

    public partial class BankTransfer
    {
        public static BankTransfer BankTransferFromJson(string json)
        {
            return JsonConvert.DeserializeObject<BankTransfer>(json, BankTransferConverter.Settings);
        }
    }
}