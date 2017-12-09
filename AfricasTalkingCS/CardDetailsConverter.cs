// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardDetailsConverter.cs" company="Africa's Talking">
//   2017
// </copyright>
// <summary>
//   The converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AfricasTalkingCS
{
    using Newtonsoft.Json;

    /// <summary>
    /// The converter.
    /// </summary>
    public static class CardDetailsConverter
    {
        /// <summary>
        /// The settings.
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
                                                                     {
                                                                         MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                                                                         DateParseHandling = DateParseHandling.None,
                                                                     };
    }
}