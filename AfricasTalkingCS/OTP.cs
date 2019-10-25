using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public partial class OTP
    {
        /// <summary>
        ///     Gets or sets the OTP.
        ///     This contains the One Time Password that the card issuer sent to the client that owns the payment card
        /// </summary>
        [JsonProperty("otp")]
        public string Otp { get; set; }

        /// <summary>
        ///     Gets or sets the transaction id.
        ///     This value identifies the transaction that your application wants to validate. This value is contained in the
        ///     response to the charge request.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        ///     This is your Africa's Talking username
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class OTP
    {
        /// <summary>
        ///     Deserialize OTP.
        /// </summary>
        /// <param name="json">
        ///     The JSON object.
        /// </param>
        /// <returns>
        ///     The <see cref="OTP" />.
        /// </returns>
        public static OTP OTPFromJson(string json)
        {
            return JsonConvert.DeserializeObject<OTP>(json, OTPConverter.Settings);
        }
    }
}