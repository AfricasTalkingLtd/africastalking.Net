using AfricasTalkingCS;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace AfricasTalkingCS_Tests
{
    class AirtimeUsers {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }


    public class AirtimeService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        [Fact]
        public void DoSendAirtimeToOneUser()
        {
            var airtimeUser = new AirtimeUsers();
            airtimeUser.PhoneNumber = "+254720000000";
            airtimeUser.Amount = "KES 100";
            var airtimeRec = JsonConvert.SerializeObject(airtimeUser);
            var gatewayResponse = _atGWInstance.SendAirtime(airtimeRec);
            var success = gatewayResponse["errorMessage"] == "None" || gatewayResponse["errorMessage"] == "A duplicate request was received within the last 5 minutes";
            Assert.True(success);
        }


        [Fact]
        public void DoSendToManyUsers()
        {
            var airtimeUser1 = new AirtimeUsers();
            airtimeUser1.PhoneNumber = "+254720000001";
            airtimeUser1.Amount = "KES 100";
            string airtime1Recipient = JsonConvert.SerializeObject(airtimeUser1);
            var airtimeUser2 = new AirtimeUsers();
            airtimeUser2.PhoneNumber = "+254720000002";
            airtimeUser2.Amount = "KES 100";
            string airtime2Recipient = JsonConvert.SerializeObject(airtimeUser2);
            StringBuilder airtimeStringBuilderInstance = new StringBuilder(airtime1Recipient, 100);
            // Hack
            airtimeStringBuilderInstance.Append($",{airtime2Recipient}");
            var gatewayResponse = _atGWInstance.SendAirtime(airtimeStringBuilderInstance);
            var success = gatewayResponse["errorMessage"] == "None" || gatewayResponse["errorMessage"] == "A duplicate request was received within the last 5 minutes";
            Assert.True(success);
        }
    }
}