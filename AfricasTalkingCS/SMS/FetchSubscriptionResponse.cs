using AfricasTalkingSDK.SMS;
using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class FetchSubscriptionResponse
    {
        public SMSMessageData Data { get; set; }
        public class SMSMessageData
        {
            public List<Subscription> subscriptions;
        }
    }
}
