// --------------------------------------------------------------------------------------------------------------------
// <copyright file="C2BDataResults.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the C2BDataResults Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The C2B response result.
    /// </summary>
    public class C2BDataResults
    {
        /// <summary>
        ///     Gets or sets the Provider Channel.
        /// </summary>
        [JsonProperty("providerChannel")]
        public string ProviderChannel { get; set; }

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