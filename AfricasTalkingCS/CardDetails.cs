using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricasTalkingCS
{
    public class CardDetails
    {
     // [JsonProperty("number")]

        public string number { get; set; }

     // [JsonProperty("countryCode")]
        public string countryCode { get; set; }

     // [JsonProperty("cvvNumber")]

        public short cvvNumber { get; set; }

     //  [JsonProperty("expiryMonth")]

        public string expiryMonth { get; set; }

     // [JsonProperty("expiryYear")] 

        public string expiryYear { get; set; }

     // [JsonProperty("authToken")]

        public string authToken { get; set; }

        public CardDetails(string cardNumber, string countryCode, short cVVNumber, string expiryMonth, string expiryYear, string authToken)
        {
            this.number = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
            this.countryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
            this.cvvNumber = cVVNumber;
            this.expiryMonth = expiryMonth ?? throw new ArgumentNullException(nameof(expiryMonth));
            this.expiryYear = expiryYear ?? throw new ArgumentNullException(nameof(expiryYear));
            this.authToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
        }
        //public string ToJson()
        //{
        //    var json = JsonConvert.SerializeObject(this);
        //    return json;
        //}
    }
}
