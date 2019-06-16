using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Text;

namespace AfricasTalkingCS.Tests
{
    class AirtimeUsers {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

    [TestClass]
    public class AirtimeService : TestBase
    {
        [TestMethod]
        public void DoSendAirtimeToOneUser()
        {
            var airtimeUser = new AirtimeUsers
            {
                PhoneNumber = phoneNumber0,
                Amount = amount100
            };
            var airtimeRec = airtimeUser.ToJson();
            var gatewayResponse = _atGWInstance.SendAirtime(airtimeRec);
            var success = gatewayResponse["errorMessage"] == "None" || gatewayResponse["errorMessage"] == "A duplicate request was received within the last 5 minutes";
            Assert.IsTrue(success);
        }


        [TestMethod]
        public void DoSendToManyUsers()
        {
            var airtimeUser1 = new AirtimeUsers
            {
                PhoneNumber = phoneNumber0,
                Amount = amount100
            };
            string airtime1Recipient = airtimeUser1.ToJson();
            var airtimeUser2 = new AirtimeUsers
            {
                PhoneNumber = phoneNumber2,
                Amount = amount100
            };
            string airtime2Recipient = airtimeUser2.ToJson();
            StringBuilder airtimeStringBuilderInstance = new StringBuilder(airtime1Recipient, 100);
            // Hack
            airtimeStringBuilderInstance.Append($",{airtime2Recipient}");
            var gatewayResponse = _atGWInstance.SendAirtime(airtimeStringBuilderInstance);
            var success = gatewayResponse["errorMessage"] == "None" || gatewayResponse["errorMessage"] == "A duplicate request was received within the last 5 minutes";
            Assert.IsTrue(success);
        }
    }
}