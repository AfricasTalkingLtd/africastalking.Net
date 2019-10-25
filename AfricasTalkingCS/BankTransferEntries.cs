// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankTransferEntries.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankTransferEntries Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The Bank Transfer Entries response result.
    /// </summary>
    public class BankTransferEntries
    {
        /// <summary>
        ///     Gets or sets the Account Number.
        /// </summary>
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        /// <summary>
        ///     Gets or sets the Transaction Fee.
        /// </summary>
        [JsonProperty("transactionFee")]
        public string TransactionFee { get; set; }

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
        ///     Sets/ Gets Error Message.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

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