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


        public string Send(string message, List<string> recipients)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "to", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            return response;
        }
        public string Send(string message, List<string> recipients, bool enqueue)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "enqueue", enqueue ? "1" : "0" },
                { "to", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            return response;
        }

        public string Send(string message, List<string> recipients, string from)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "from", from },
                { "to", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            return response;
        }

        public string Send(string message, List<string> recipients, string from, bool enqueue)
        {
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "from", from },
                { "enqueue", enqueue ? "1" : "0" },
                { "to", recipientsString }
            };
            var response = MakeRequest("messaging", "POST", requestData);
            return response;
        }

        public string SendPremium(string message, string keyword, string linkId, List<string> recipients, string senderId, int retryDurationInHours)
        {
            var retryDuration = retryDurationInHours <= 0 ? null : retryDurationInHours.ToString();
            var recipientsString = string.Join(",", recipients.ToArray());
            var requestData = new Dictionary<string, string>
            {
                { "message", message },
                { "to", recipientsString },
                { "keyword", keyword },
                { "linkId", linkId },
                { "retryDuration", retryDuration },
                { "enqueue", "0" }
            };
            var response = MakeRequest("send", "POST", requestData);

            var smsResponse = JsonConvert.DeserializeObject(response);
            return smsResponse as string;
        }

        public string FetchMessages(int lastReceivedId)
        {
            var requestData = new Dictionary<string, string>
            {
                { "lastReceivedId", lastReceivedId.ToString() }
            };
            var response = MakeRequest("messaging", requestData);
            return response;
        }

        public string FetchSubscriptions(string shortCode, string keyword, int lastReceivedId)
        {
            var requestData = new Dictionary<string, string>
            {
                { "shortCode", shortCode },
                { "lastReceivedId", lastReceivedId.ToString() },
                { "keyword", keyword }
            };
            var response = MakeRequest("subscription", requestData);
            return response;
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
