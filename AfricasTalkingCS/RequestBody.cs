using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    internal class RequestBody
    {
        public RequestBody()
        {
            recepients = new List<MobileB2CRecepient>();
        }
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("productName")]
        public string productName { get; set; }
        [JsonProperty("recipients")]
        public List<MobileB2CRecepient> recepients { get; set; }

        public override string ToString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}