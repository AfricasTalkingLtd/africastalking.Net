using System;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public class BankAccountDetails
    {
        [JsonProperty("accountName")]
        public string AccountName { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
        [JsonProperty("bankCode")]
        public string BankCode { get; set; }
        public BankAccountDetails(string accountName, string accountNumber, string bankCode)
        {
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            BankCode = bankCode ?? throw new ArgumentNullException(nameof(bankCode));
        }
        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
