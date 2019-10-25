// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankCheckoutConverter.cs" company="Africa's Talking">
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
    ///     The bank checkout JSON converter.
    /// </summary>
    public static class BankCheckoutConverter
    {
        /// <summary>
        ///     The serializer settings.
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}