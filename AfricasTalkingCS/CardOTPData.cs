using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public class CardOTPData
    {
       // [JsonProperty("username")]
        public string username { get; set; }
     //   [JsonProperty("transactionId")]
        public string transactionId { get; set; }
      //  [JsonProperty("otp")]
        public string otp { get; set; }
    }
}
