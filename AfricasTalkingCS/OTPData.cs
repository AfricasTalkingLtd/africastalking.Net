using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public class OTPData
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("transactionId")]
        public string TransactionID { get; set; }
        [JsonProperty("otp")]
        public string OTP { get; set; }
    }
}
