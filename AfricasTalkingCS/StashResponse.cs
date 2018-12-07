// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StashResponse.cs" company="Africa's Talking">
//   2018
// </copyright>
// <summary>
//   Defines the StashResponse Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The Wallet transfer / Stash Topup response result.
    /// </summary>
    public class StashResponse
    {
        /// <summary>
        /// Gets or sets the Transaction ID.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Transaction ID.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

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