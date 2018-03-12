using System;
using System.Collections.Generic;
using System.Text;

namespace AfricasTalkingSDK.SMS
{
    public  class SendMessageResponse
    {
        public SMSMessageData Data { get; set; }
        public class SMSMessageData
        {
            public List<SMSRecipient> recipients;
        }
    }
}
