using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricasTalkingCS
{
    class OTPData
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("transactionId")]
        public string TransactionID { get; set; }
        [JsonProperty("otp")]
        public string OTP { get; set; }
    }
}
