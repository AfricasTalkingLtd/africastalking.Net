using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    internal class RequestBody
    {
        public RequestBody()
        {
            this.Recepients = new List<MobileB2CRecepient>();
        }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("recipients")]
        public List<MobileB2CRecepient> Recepients { get; set; }

        public override string ToString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}