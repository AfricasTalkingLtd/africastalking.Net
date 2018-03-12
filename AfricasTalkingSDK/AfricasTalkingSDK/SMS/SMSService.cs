using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingSDK.SMS
{
    
    public class SmsService : Service
    {
        private dynamic _instance;
        public SmsService(string username, string apiKey) : base(username, apiKey, "sms") { }

        protected override dynamic GetInstance(string username, string apiKey)
        {
            if (_instance == null)
            {
                _instance = new SmsService(username, apiKey);
            }
            return _instance;
        }


        public List<SMSRecipient> Send(string message, List<string> recipients) => Send(message, recipients, enqueue: false);

        public List<SMSRecipient> Send(string message, List<string> recipients, bool enqueue)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "from", null },
                { "enqueue", enqueue ? "1" : "0" },
                { "recipients", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            var smsResponse = JsonConvert.DeserializeObject<SendMessageResponse>(response);
            return smsResponse.Data.recipients;
        }

        public List<SMSRecipient> Send(string message, List<string> recipients, string from, bool enqueue)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "from", from ?? from },
                { "enqueue", enqueue ? "1" : "0" },
                { "recipients", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            var smsResponse = JsonConvert.DeserializeObject<SendMessageResponse>(response);
            return smsResponse.Data.recipients;
        }

        public List<SMSRecipient> SendPremium(string message, string keyword, string linkId, List<string> recipients, string senderId, int retryDurationInHours)
        {
            var retryDuration = retryDurationInHours <= 0 ? null : retryDurationInHours.ToString();
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "recipients", recipientsString },
                { "keyword", keyword },
                { "linkId", linkId },
                { "retryDuration", retryDuration },
                { "enqueue", "0" }
            };
            var response = MakeRequest("send", "POST", requestData);

            var smsResponse = JsonConvert.DeserializeObject<SendMessageResponse>(response);
            return smsResponse.Data.recipients;
        }

        public List<Message> FetchMessages(int lastReceivedId)
        {
            var requestData = new Dictionary<string, string>
            {
                { "lastReceivedId", lastReceivedId.ToString() }
            };
            var response = MakeRequest("messaging", requestData);
            var fetchResponse = JsonConvert.DeserializeObject<FetchMessageResponse>(response);
            return fetchResponse.Data.messages;
        }

        public List<Subscription> FetchSubscriptions(string shortCode, string keyword, int lastReceivedId)
        {
            var requestData = new Dictionary<string, string>
            {
                { "shortCode", shortCode },
                { "lastReceivedId", lastReceivedId.ToString() },
                { "keyword", keyword }
            };
            var response = MakeRequest("subscription", requestData);
            var fetchResponse = JsonConvert.DeserializeObject<FetchSubscriptionResponse>(response);
            return fetchResponse.Data.subscriptions;
        }

        public SubscriptionResponse CreateSubscription(string shortCode, string keyword, string phoneNumber, string checkoutToken)
        {
            try
            {
                CheckPhoneNumber(phoneNumber);
                var requestData = new Dictionary<string, string>
                {
                    { "phoneNumber", phoneNumber },
                    { "shortCode", shortCode },
                    { "keyword", keyword },
                    { "checkoutToken", checkoutToken }
                };
                var response = MakeRequest("subscription/create", "POST", requestData);
                var subscriptionResponse = JsonConvert.DeserializeObject<SubscriptionResponse>(response);
                return subscriptionResponse;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }

}
