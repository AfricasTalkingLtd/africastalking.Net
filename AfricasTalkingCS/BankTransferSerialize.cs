// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankTransferSerialize.cs" company="Africa's Talking">
//   2018
// </copyright>
// <summary>
//   Defines the BankTransferSerialize type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using Newtonsoft.Json;

    public static class BankTransferSerialize
    {
        /// <summary>
        /// Serializes BankTransfer object to JSON.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BankTransferToJson(this BankTransfer self) => JsonConvert.SerializeObject(self, BankTransferConverter.Settings);
    }
}