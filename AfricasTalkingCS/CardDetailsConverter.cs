// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardDetailsConverter.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   The converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The converter.
    /// </summary>
    public static class CardDetailsConverter
    {
        /// <summary>
        ///     The settings.
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}