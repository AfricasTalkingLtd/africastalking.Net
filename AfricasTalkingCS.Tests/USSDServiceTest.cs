using Microsoft.VisualStudio.TestTools.UnitTesting;
using AfricasTalkingCS;

namespace AfricasTalkingCS_Tests
{
    [TestClass]
    public class UssdPushService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        [TestMethod]
        public void DoSendUssdPush()
        {
            var phoneNumber = "+254720000002";
            var menu = "CON What is your purpose?\n";
            var checkoutToken = _atGWInstance.CreateCheckoutToken(phoneNumber);
            string tkn = checkoutToken["token"];
            var gatewayResponse = _atGWInstance.InitiateUssdPushRequest(phoneNumber, menu, tkn);
            var success = gatewayResponse["status"] == "Queued" && gatewayResponse["errorMessage"] == "None";
            Assert.IsTrue(success, "Should successfully initiate a USSS Push request");
        }
    }
}