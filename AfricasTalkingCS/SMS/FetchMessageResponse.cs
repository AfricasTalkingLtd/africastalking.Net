using AfricasTalkingSDK.SMS;
using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class FetchMessageResponse
    {
        public SMSMessageData Data { get; set; }
        public class SMSMessageData
        {
            public List<Message> messages;
        }
    }
}
