using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AfricasTalkingCS.Tests
{
    [TestClass]
    public class UssdPushService : TestBase
    {
        [TestMethod]
        public void DoSendUssdPush()
        {
            var phoneNumber = phoneNumber2;
            var menu = "CON What is your purpose?\n";
            var checkoutToken = _atGWInstance.CreateCheckoutToken(phoneNumber);
            string tkn = checkoutToken["token"];
            var gatewayResponse = _atGWInstance.InitiateUssdPushRequest(phoneNumber, menu, tkn);
            var success = gatewayResponse["status"] == "Queued" && gatewayResponse["errorMessage"] == "None";
            Assert.IsTrue(success, "Should successfully initiate a USSS Push request");
        }
    }
}