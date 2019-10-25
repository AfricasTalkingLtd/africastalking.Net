using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    public static class ObjConverter
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