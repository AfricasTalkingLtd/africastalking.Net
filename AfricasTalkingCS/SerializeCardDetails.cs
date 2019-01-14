// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializeCardDetails.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   The serialize.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AfricasTalkingCS
{
    using Newtonsoft.Json;

    /// <summary>
    /// The serialize.
    /// </summary>
    public static class SerializeCardDetails
    {
        /// <summary>
        /// Serializes Card details to JSON objects.
        /// </summary>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CardDetailsToJson(this CardDetails self) => JsonConvert.SerializeObject(self, CardDetailsConverter.Settings);
    }
}