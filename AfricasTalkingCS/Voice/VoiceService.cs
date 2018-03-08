using AfricasTalkingSDK.Voice;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class VoiceService : Service
    {
        private dynamic _instance;
        VoiceService(string username, string apiKey) : base(username, apiKey, "voice", "voice") { }

        protected override dynamic GetInstance(string username, string apiKey)
        {
            if (_instance == null)
            {
                _instance = new VoiceService(username, apiKey);
            }
            return _instance;
        }

        public CallResponse Call(string phoneNumber)
        {
            try 
            {
                CheckPhoneNumber(phoneNumber);
                var requestData = new Dictionary<string, string>
                {
                    { "from", "" },
                    { "to", phoneNumber }
                };
                var response = MakeRequest("call", "POST", requestData);
                CallResponse callResponse = JsonConvert.DeserializeObject<CallResponse>(response);
                return callResponse;
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CallResponse Call(string callTo, string callFrom)
        {
            try
            {
                CheckPhoneNumber(callTo);
                var requestData = new Dictionary<string, string>
                {
                    { "from", callFrom },
                    { "to", callTo}
                };
                var response = MakeRequest("call", "POST", requestData);
                CallResponse callResponse = JsonConvert.DeserializeObject<CallResponse>(response);
                return callResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public QueuedCallsResponse FetchQueuedCalls(string phoneNumber)
        {
            try
            {
                CheckPhoneNumber(phoneNumber);
                var requestData = new Dictionary<string, string>
                {
                    { "phoneNumbers", phoneNumber }
                };
                var response = MakeRequest("queueStatus", "POST", requestData);
                QueuedCallsResponse queuedCallsResponse = JsonConvert.DeserializeObject<QueuedCallsResponse>(response);
                return queuedCallsResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string MediaUpload(string phoneNumber, string url)
        {
            try
            {
                CheckPhoneNumber(phoneNumber);
                var requestData = new Dictionary<string, string>
                {
                    { "phoneNumbers", phoneNumber },
                    { "url", url }
                };
                var response = MakeRequest("mediaUpload", "POST", requestData);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
