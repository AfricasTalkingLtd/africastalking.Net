using AfricasTalkingCS;
using Xunit;

namespace AfricasTalkingCS_Tests
{
    public class SMSService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private static AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        [Fact]
        public void DoSendMessageToOneValidNumber()
        {
            var phoneNumber = "+254720000000";
            var message     = "Hello Mr. Anderson";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumber, message);
            var success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            Assert.True(success, "Should successfully send message to a valid phone number");
        }

        [Fact]
        public void DoSendMessageToManyPeopleofValidNumbers()
        {
            var phoneNumerList  = "+254720000001,+254720000002,+254720000003";
            var message         = "Good Evening Mr. Smith";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumerList, message);
            // Avoid loops
            var recipient1Success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            var recipient2Success = gatewayResponse["SMSMessageData"]["Recipients"][1]["status"] == "Success";
            var recipient3Success = gatewayResponse["SMSMessageData"]["Recipients"][2]["status"] == "Success";
            var success = recipient1Success && recipient2Success && recipient3Success;
            Assert.True(success, "Should successfully send a message to multiple recipients");
        }

        [Fact] 
        public void DoSendMessageViaSenderId() 
        {
            var phoneNumber = "+254720000000";
            var message = "Hello Mr. Wick";
            var senderID = "Coolguy";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumber, message, senderID);
            var success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            Assert.True(success, "Should send message to avalid phoneNumber via SenderID");
        }

        [Fact]
        public void DoSendMessageViaShortCode()
        {
            var phoneNumber = "+254720000000";
            var message = "Hello Neo";
            var shortCode = "44000";
            var gatewayResponse = _atGWInstance.SendMessage(phoneNumber, message, shortCode);
            var success = gatewayResponse["SMSMessageData"]["Recipients"][0]["status"] == "Success";
            Assert.True(success, "Should send message to avalid phoneNumber via Shortcode");
        }

        [Fact]
        public void DoCreateCheckoutToken()
        {
            var phoneNumber = "+254720000000";
            var gatewayResponse = _atGWInstance.CreateCheckoutToken(phoneNumber);
            var success = gatewayResponse["token"];
            // "Should successfully create a checkout token for any valid number"
            Assert.NotNull(success);
        }

        [Fact]
        public void DoCreateSubscription()
        {
            var phoneNumber = "+254720000000";
            var shortCode = "44000";
            var keyword = "Coolguy";
            var getToken = _atGWInstance.CreateCheckoutToken(phoneNumber);
            string token = getToken["token"];
            var gatewayResponse = _atGWInstance.CreateSubscription(phoneNumber, shortCode, keyword, token);
            var success = gatewayResponse["status"] == "Success";
            Assert.True(success);
        }

        [Fact]
        public void DoDeleteSubscription()
        {
            var phoneNumber = "+254720000000";
            var shortCode = "44000";
            var keyword = "Coolestguy";
            var getToken = _atGWInstance.CreateCheckoutToken(phoneNumber);
            string token = getToken["token"];
            var subscribeUser = _atGWInstance.CreateSubscription(phoneNumber, shortCode, keyword, token);
            var deleteUserSub = _atGWInstance.DeleteSubscription(phoneNumber, shortCode, keyword);
            // Should be mocked
            var success = deleteUserSub["description"] == "Succeeded";
            Assert.True(success, "Should successfully delete a subscription");
        }

    }
    
}
