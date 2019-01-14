// --------------------------------------------------------------------------------------------------------------------
// <copyright file="B2BResult.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the B2BResult Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The B2B response result.
    /// </summary>
    public class B2BResult
    {
        /// <summary>
        /// Gets or sets the Provider Channel.
        /// </summary>
        [JsonProperty("providerChannel")]
        public int ProviderChannel { get; set; }

        /// <summary>
        /// Gets or sets the Transaction ID.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Fee.
        /// </summary>
        [JsonProperty("transactionFee")]
        public string TransactionFee { get; set; }

        /// <summary>
        /// Sets/ Gets status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var result = JsonConvert.SerializeObject(this);
            return result;
        }
    }
}