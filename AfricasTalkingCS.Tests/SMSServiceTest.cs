using Microsoft.VisualStudio.TestTools.UnitTesting;
using AfricasTalkingCS;

namespace AfricasTalkingCS_Tests
{
    [TestClass]
    public class SMSService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        [TestMethod]
        public void SendsMessageToOneValidNumber()
        {
            var phoneNumber = "+254720000000";
            var message     = "Hello Mr. Anderson";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumber, message);
            var success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            Assert.IsTrue(success, "Should successfully send message to a valid phone number");
        }

        [TestMethod]
        public void SendsMessageToManyPeopleofValidNumbers()
        {
            var phoneNumerList  = "+254720000001,+254720000002,+254720000003";
            var message         = "Good Evening Mr. Smith";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumerList, message);
            // Avoid loops
            var recipient1Success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            var recipient2Success = gatewayResponse["SMSMessageData"]["Recipients"][1]["status"] == "Success";
            var recipient3Success = gatewayResponse["SMSMessageData"]["Recipients"][2]["status"] == "Success";
            var success = recipient1Success && recipient2Success && recipient3Success;
            Assert.IsTrue(success, "Should successfully send a message to multiple recipients");
        }


    }
}
