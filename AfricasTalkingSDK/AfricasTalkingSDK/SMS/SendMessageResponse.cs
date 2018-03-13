using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AfricasTalkingSDK.SMS
{
    
    public partial class SendMessageResponse
    {
        [JsonProperty("SMSMessageData")]
        public SmsMessageData SmsMessageData { get; set; }
    }

    public partial class SmsMessageData
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Recipients")]
        public Recipient[] Recipients { get; set; }
    }

    public partial class Recipient
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }

}
