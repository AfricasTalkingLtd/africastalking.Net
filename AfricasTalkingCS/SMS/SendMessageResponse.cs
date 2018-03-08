using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class SendMessageResponse
    {
        public SMSMessageData Data { get; set; }
        public class SMSMessageData
        {
            public List<SMSRecipient> recipients;
        }
    }
}
