// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializeBankCheckout.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankChecout type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The serialize bank checkout class.
    /// </summary>
    public static class SerializeBankCheckout
    {
        /// <summary>
        ///     The bank checkout-to-JSON converter.
        /// </summary>
        /// <param name="self">
        ///     The self.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string BankCheckoutToJson(this BankCheckout self)
        {
            return JsonConvert.SerializeObject(self, BankCheckoutConverter.Settings);
        }
    }
}