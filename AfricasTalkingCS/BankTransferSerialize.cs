// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankTransferSerialize.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankTransferSerialize type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public static class BankTransferSerialize
    {
        /// <summary>
        ///     Serializes BankTransfer object to JSON.
        /// </summary>
        /// <param name="self">
        ///     The self.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string BankTransferToJson(this BankTransfer self)
        {
            return JsonConvert.SerializeObject(self, BankTransferConverter.Settings);
        }
    }
}