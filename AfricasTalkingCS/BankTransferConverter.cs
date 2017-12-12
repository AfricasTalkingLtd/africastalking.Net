
namespace AfricasTalkingCS
{

    using Newtonsoft.Json;

    public class BankTransferConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
                                                                     {
                                                                         MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                                                                         DateParseHandling = DateParseHandling.None,
                                                                     };
    }
}
