// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankCheckoutResponse.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankCheckoutResponse Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The BankCheckout response result.
    /// </summary>
    public class BankCheckoutResponse
    {
        /// <summary>
        ///     Gets or sets the Transaction ID.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the Transaction ID.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        ///     Sets/ Gets status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            var result = JsonConvert.SerializeObject(this);
            return result;
        }
    }
}