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
        [JsonProperty("number")]
        public string CardNumber { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("cvvNumber")]
        public string CVVNumber { get; set; }
        [JsonProperty("expiryMonth")]
        public string ExpiryMonth { get; set; }
        [JsonProperty("expiryYear")]
        public string ExpiryYear { get; set; }
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        public CardDetails(string cardNumber, string countryCode, string cVVNumber, string expiryMonth, string expiryYear, string authToken)
        {
            CardNumber = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
            CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
            CVVNumber = cVVNumber ?? throw new ArgumentNullException(nameof(cVVNumber));
            ExpiryMonth = expiryMonth ?? throw new ArgumentNullException(nameof(expiryMonth));
            ExpiryYear = expiryYear ?? throw new ArgumentNullException(nameof(expiryYear));
            AuthToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
        }
        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
