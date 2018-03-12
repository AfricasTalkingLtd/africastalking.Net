using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingSDK.Airtime
{
    class AirtimeService : Service
    {
        private dynamic _instance;

        public AirtimeService(string username, string apiKey) : base(username, apiKey, "airtime") { }

        protected override dynamic GetInstance(string username, string apiKey)
        {
            if (_instance == null)
            {
                _instance = new AirtimeService(username, apiKey);
            }
            return _instance;
        }

        public AirtimeResponse Send(string phone, string amount)
        {
            try
            {

                var recipientsList = new Dictionary<string, string>
                {
                    { phone, amount }
                };
                var response = MakeRequest("send", recipientsList);
                var airtimeResponse = JsonConvert.DeserializeObject<AirtimeResponse>(response);
                return airtimeResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public AirtimeResponse Send(Dictionary<string, string> recipientsList)
        {
            var response = MakeRequest("send", "POST", recipientsList);
            var airtimeResponse = JsonConvert.DeserializeObject<AirtimeResponse>(response);
            return airtimeResponse;
        }
    }
}
