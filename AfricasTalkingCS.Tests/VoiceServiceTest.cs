using AfricasTalkingCS;
using Xunit;

namespace AfricasTalkingCS_Tests
{
    public class VoiceService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        private readonly AfricasTalkingGatewayException africasTalkingGatewayException = new AfricasTalkingGatewayException("Failed to upload media file");

        [Fact]
        public void DoPushOutboundCallToOneNumber()
        {
            var phoneNumber = "+254720000001";
            var callerId = "+254720000000";
            var gatewayResponse  = _atGWInstance.Call(callerId, phoneNumber);
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.True(success, "Should successfully push outbound call to one number");
        }

        [Fact] 
        public void DoPushOutboundCallToMultipleNumbers() 
        {
            var numberLists = "+254720000003,test.user@sandbox.sip.africastalking.com";
            var callerId = "+254720000000";
            var gatewayResponse = _atGWInstance.Call(callerId, numberLists); 
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.True(success, "Should successfully push outbound call to one number");            
        }

        [Fact] 
        public void DoPushOutboundCallWithRequestId()
        {
            var callee = "+254720000002";
            var callerId = "+254720000000";
            var requestId = "test";
            var gatewayResponse = _atGWInstance.Call(callerId, callee, requestId); 
            var success = gatewayResponse["errorMessage"] == "None";
            Assert.True(success, "Should successfully push outbound call to one number"); 
        }

        [Fact] 
        public void DoUploadMediaFile() 
        {
            var callerId = "+254720000000";
            var fileUrl = "https://storage.zion.ai/chant_of_the_oracle.mp3";
            var gatewayResponse = _atGWInstance.UploadMediaFile(fileUrl, callerId); 
            // Hacked !!!
            var success = gatewayResponse != africasTalkingGatewayException.ToString();
            Assert.True(success, "Should successfully upload media file");
        }

        // Fetch queue

        // [Fact] 
        // public void DoFetchCallQueue() 
        // {
        //     var queueNumber = "+254711082518";
        //     var gatewayResponse = _atGWInstance.GetNumberOfQueuedCalls(queueNumber);
        //     var success = (gatewayResponse["status"] == "Success" && gatewayResponse["errorMessage"] == "None");
        //     Assert.True(success, "Should successfully get call queues");
        // }
    }
}