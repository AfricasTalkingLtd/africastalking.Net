using System.Collections.Generic;

namespace AfricasTalkingSDK.SMS
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
