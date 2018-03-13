using System.Collections.Generic;
using AfricasTalkingSDK.SMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAfricasTalkingSDK
{
    [TestClass]
    public class UnitTestSmsSendService
    {
         static string username = "sandbox";
         static string apikey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";

        private readonly SmsService _sms = new SmsService(username, apikey);
        private readonly List<string> _phoneList = new List<string>(new[]{"+254724587654","+254724545678","+254712886320","+254718963241","+254715896520"});
        
        [TestMethod]
        public void TestSendSms()
        {
            const string message = "Hello .NET";
            var response = _sms.Send(message, _phoneList);
            bool aseertSuccess = response.Contains("Success");
            Assert.IsTrue(aseertSuccess);
        }

        [TestMethod]
        public void TestEnque()
        {
            const string message = "Hello Enque Message";
            var response = _sms.Send(message, _phoneList, true);
            var assertEnque = response.Contains("Success");
            Assert.IsTrue(assertEnque);
        }

        [TestMethod]
        public void TestEnqueueShortCode()
        {
            const string message = "Hello Enque Message";
            const string shortCode = "44005";
            var response = _sms.Send(message, _phoneList, shortCode, true);
            var assertEnque = response.Contains("Success");
            Assert.IsTrue(assertEnque);
        }

        [TestMethod]
        public void TestEnqueueSenderId()
        {
            const string message = "Hello Enque Message";
            const string senderId = "Coolguy";
            var response = _sms.Send(message, _phoneList, senderId, true);
            var assertEnque = response.Contains("Success");
            Assert.IsTrue(assertEnque);
        }

        [TestMethod]
        public void TestShortCode()
        {
            const string message = "Hello From Sender ID/ ShortCode";
            const string shortCode = "44005";
            var response = _sms.Send(message, _phoneList, shortCode);
            var assertShortCodeResponse = response.Contains("Success");
            Assert.IsTrue(assertShortCodeResponse);
        }

        [TestMethod]
        public void TestSenderId()
        {
            const string message = "Hello From SenderID";
            const string senderId = "Coolguy";
            var response = _sms.Send(message, _phoneList, senderId);
            var assertSenderIdResponse = response.Contains("Success");
            Assert.IsTrue(assertSenderIdResponse);
        }
    }

}
