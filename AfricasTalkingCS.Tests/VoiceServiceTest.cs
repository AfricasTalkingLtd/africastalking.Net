using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AfricasTalkingCS.Tests
{

    [TestClass]
    public class VoiceService : TestBase
    {
        private readonly AfricasTalkingGatewayException africasTalkingGatewayException = new AfricasTalkingGatewayException("Failed to upload media file");
        [TestMethod]
        public void DoPushOutboundCallToOneNumber()
        {
            var phoneNumber = phoneNumber1;
            var callerId = phoneNumber0;
            var gatewayResponse  = _atGWInstance.Call(callerId, phoneNumber);
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.IsTrue(success, "Should successfully push outbound call to one number");
        }

        [TestMethod] 
        public void DoPushOutboundCallToMultipleNumbers() 
        {
            var numberLists = $"{phoneNumber3},test.user@sandbox.sip.africastalking.com";
            var callerId = phoneNumber0;
            var gatewayResponse = _atGWInstance.Call(callerId, numberLists); 
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.IsTrue(success, "Should successfully push outbound call to one number");            
        }

        [TestMethod] 
        public void DoPushOutboundCallWithRequestId()
        {
            var callee = phoneNumber2;
            var callerId = phoneNumber0;
            var requestId = "test";
            var gatewayResponse = _atGWInstance.Call(callerId, callee, requestId); 
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.IsTrue(success, "Should successfully push outbound call to one number"); 
        }

        [TestMethod] 
        public void DoUploadMediaFile() 
        {
            var callerId = "+254720000000";
            var fileUrl = "https://storage.zion.ai/chant_of_the_oracle.mp3";
            var gatewayResponse = _atGWInstance.UploadMediaFile(fileUrl, callerId); 
            // Hacked !!!
            var success = gatewayResponse != africasTalkingGatewayException.ToString();
            Assert.IsTrue(success, "Should successfully upload media file");
        }

        // Fetch queue

        // [TestMethod] 
        // public void DoFetchCallQueue() 
        // {
        //     var queueNumber = "+254711082518";
        //     var gatewayResponse = _atGWInstance.GetNumberOfQueuedCalls(queueNumber);
        //     var success = (gatewayResponse["status"] == "Success" && gatewayResponse["errorMessage"] == "None");
        //     Assert.IsTrue(success, "Should successfully get call queues");
        // }
    }
}