// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankCheckout.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankChecout type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Bank Account checkout APIs allow your application to collect money into your Payment Wallet by initiating an OTP-validated transaction that deducts money from a customer's bank account.
    /// </summary>
    public partial class BankCheckout
    {
        /// <summary>
        /// Gets or sets the amount.
        /// This is the amount (in the provided currency) that the mobile subscriber is expected to confirm.
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the bank account.
        /// This is a complex type whose structure is described below. It contains the details of the bank account to be charged in this transaction.
        /// </summary>
        [JsonProperty("bankAccount")]
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// This is the 3-digit ISO format currency code for the value of this transaction (e.g NGN, USD, KES etc).
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// This value contains a map of any metadata that you would like us to associate with this request. You can use this field to send data that will map notifications to checkout requests, since we will include it when we send notifications once the checkout is complete.
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
        /// Gets or sets the product name.
        /// This value identifies the Africa's Talking Payment Product that should be used to initiate this transaction.
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// This is your Africa's Talking username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    /// <summary>
    /// The bank checkout.
    /// </summary>
    public partial class BankCheckout
    {
        /// <summary>
        /// The bank checkout JSON de-serializer method.
        /// </summary>
        /// <param name="json">
        /// The JSON.
        /// </param>
        /// <returns>
        /// The <see cref="BankCheckout"/>.
        /// </returns>
        public static BankCheckout BankChecoutFromJson(string json) => JsonConvert.DeserializeObject<BankCheckout>(json, BankCheckoutConverter.Settings);
    }
}
