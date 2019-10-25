// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataResult.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the DataResult Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The data result.
    /// </summary>
    public class DataResult
    {
        /// <summary>
        ///     Gets or sets the queued items.
        /// </summary>
        [JsonProperty("numQueued")]
        public int NumQueued { get; set; }

        /// <summary>
        ///     Gets or sets the entries.
        /// </summary>
        [JsonProperty("entries")]
        public IList<Entry> Entries { get; set; }

        /// <summary>
        ///     Gets or sets the total value.
        /// </summary>
        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        /// <summary>
        ///     Gets or sets the total transaction fee.
        /// </summary>
        [JsonProperty("totalTransactionFee")]
        public string TotalTransactionFee { get; set; }

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