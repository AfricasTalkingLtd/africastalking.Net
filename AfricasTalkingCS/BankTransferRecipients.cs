// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankTransferRecipients.cs" company="Africa's Talking">
//   2018
// </copyright>
// <summary>
//   This contains a list of Recipient elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// This contains a list of Recipient elements.
    /// </summary>
    public partial class BankTransferRecipients
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankTransferRecipients"/> class.
        /// </summary>
        /// <param name="amount">
        /// This is the amount (in the provided currency) that the mobile subscriber is expected to receive.
        /// </param>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <param name="currencyCode">
        /// This is the 3-digit ISO format currency code for the value of this transaction (e.g NGN, USD, KES etc).
        /// </param>
        /// <param name="narration">
        /// A short description of the transaction that can be displayed on the client's statement.
        /// </param>
        public BankTransferRecipients(decimal amount, BankAccount bankAccount, string currencyCode, string narration)
        {
            this.Amount = amount;
            this.BankAccount = bankAccount ?? throw new ArgumentNullException(nameof(bankAccount));
            this.CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
            this.Metadata = new Dictionary<string, string>();
            this.Narration = narration ?? throw new ArgumentNullException(nameof(narration));
        }

        /// <summary>
        /// Gets or sets the amount.
        /// This is the amount (in the provided currency) that the mobile subscriber is expected to receive.
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the bank account.
        /// </summary>
        [JsonProperty("bankAccount")]
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// This is the 3-digit ISO format currency code for the value of this transaction (e.g NGN, USD, KES etc) 
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// This value contains a map of any metadata that you would like us to associate with this request. You can use this field to send data that will map notifications to checkout requests, since we will include it when we send notifications once the transaction is complete.
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the narration.
        /// A short description of the transaction that can be displayed on the client's statement.
        /// </summary>
        [JsonProperty("narration")]
        public string Narration { get; set; }

        /// <summary>
        /// The add metadata.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddMetadata(string key, string value)
        {
            this.Metadata.Add(key, value);
        }
    }
}
