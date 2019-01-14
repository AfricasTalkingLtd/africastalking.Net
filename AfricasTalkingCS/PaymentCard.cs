// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentCard.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   The payment card class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using Newtonsoft.Json;

    /// <summary>
    /// The payment card class.
    /// </summary>
    public partial class PaymentCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentCard"/> class.
        /// This is a complex type whose structure is described below. It contains the details of the Payment Card to be charged in this transaction. Please note that you can EITHER provider this or provider a checkoutToken if you have one.
        /// </summary>
        /// <param name="authToken">
        /// The card's ATM PIN.
        /// </param>
        /// <param name="countryCode">
        /// The 2-Digit countryCode where the card was issued.
        /// </param>
        /// <param name="cvvNumber">
        /// The 3 or 4 digit Card Verification Value
        /// </param>
        /// <param name="expiryMonth">
        /// The expiration month on the card (e.g 1, 5, 12).
        /// </param>
        /// <param name="expiryYear">
        /// The expiration year on the card (e.g 2019)
        /// </param>
        /// <param name="number">
        /// The payment card number.
        /// </param>
        public PaymentCard(string authToken, string countryCode, short cvvNumber, int expiryMonth, int expiryYear, string number)
        {
            this.AuthToken = authToken;
            this.CountryCode = countryCode;
            this.CvvNumber = cvvNumber;
            this.ExpiryMonth = expiryMonth;
            this.ExpiryYear = expiryYear;
            this.Number = number;
        }

        /// <summary>
        /// Gets or sets the authentication token.
        /// The card's ATM PIN
        /// </summary>
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// The 2-Digit countryCode where the card was issued. (only NG is supported).
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the CVV number.
        /// The 3 or 4 digit Card Verification Value
        /// </summary>
        [JsonProperty("cvvNumber")]
        public short CvvNumber { get; set; }

        /// <summary>
        /// Gets or sets the expiry month.
        /// The expiration month on the card (e.g 1, 5, 12)
        /// </summary>
        [JsonProperty("expiryMonth")]
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the expiry year.
        /// The expiration year on the card (e.g 2019)
        /// </summary>
        [JsonProperty("expiryYear")]
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// The payment card number.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
