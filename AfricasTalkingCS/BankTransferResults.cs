// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankTransferResults.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankTransferResults Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The Bank Transfer response result.
    /// </summary>
    public class BankTransferResults
    {
        /// <summary>
        ///     Gets or sets the Transfer entry data.
        /// </summary>
        [JsonProperty("entries")]
        public IList<BankTransferEntries> Entries { get; set; }

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