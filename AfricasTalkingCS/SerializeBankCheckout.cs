// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializeBankCheckout.cs" company="Africa's Talking">
//   2017
// </copyright>
// <summary>
//   Defines the BankChecout type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using Newtonsoft.Json;

    /// <summary>
    /// The serialize bank checkout class.
    /// </summary>
    public static class SerializeBankCheckout
    {
        /// <summary>
        /// The bank checkout-to-JSON converter.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BankCheckoutToJson(this BankCheckout self) => JsonConvert.SerializeObject(self, BankCheckoutConverter.Settings);
    }
}
