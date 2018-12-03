using Microsoft.VisualStudio.TestTools.UnitTesting;
using AfricasTalkingCS;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AfricasTalkingCS_Tests
{
    [TestClass]
    public class PaymentService
    {
        private static string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        [TestMethod]
        public void DoMobileCheckout()
        {
            var phoneNumber = "+254720000001";
            var productName = "coolproduct";
            var currency = "KES";
            decimal amount = 1000M;
            var providerChannel = "mychannel";
            var metadata = new Dictionary<string, string>
                {
                    {"dest","oracle"}
                };
            var checkoutResponse = _atGWInstance.Checkout(productName, phoneNumber, currency, amount, providerChannel, metadata);
            var success = checkoutResponse["status"]=="PendingConfirmation";
            Assert.IsTrue(success, "Should successfully send Mobile Checkout prompt");
        }

        [TestMethod]
        public void DoBusinessToClientTransaction()
        {
            const string rec1PhoneNum = "+254720000004";
            const string rec2PhoneNum = "+254720000005";
            const string productName = "awesomeproduct";
            const string rec1Name = "Mr. Smith";
            const string rec2Name = "Seraph";
            const string currency = "KES";
            const decimal rec1Amount = 2000M;
            const decimal rec2Amount = 8050M;

            var rec1 = new MobileB2CRecepient(rec1Name, rec1PhoneNum, currency, rec1Amount);
            rec1.AddMetadata("reason","New Glasses");
            var rec2 = new MobileB2CRecepient(rec2Name, rec2PhoneNum, currency, rec2Amount);
            rec2.AddMetadata("reason", "Gift from the Oracle");

            IList<MobileB2CRecepient> recepients = new List<MobileB2CRecepient>
            {
                rec1,
                rec2
            };

            DataResult b2cresponse = _atGWInstance.MobileB2C(productName, recepients);
            var success = b2cresponse.NumQueued == 2;
            Assert.IsTrue(success,  "Should successfully disburse B2C transactions to valid phone numbers");
        }

        [TestMethod] 
        public void DoBusinessToBusinessTransaction()
        {
            const string originProduct = "awesomeproduct";
            const string destProduct   = "coolproduct";
            const decimal amount  = 200;
            const string destChannel = "mychannel";
            const string providerChannel = "Athena";
            const string currency = "KES";
            dynamic metadata = new JObject();
            metadata.originator = "Oracle";
            metadata.usage      = "simulation";
            const string transferType = "BusinessToBusinessTransfer";
            B2BResult response = _atGWInstance.MobileB2B(originProduct, providerChannel, transferType, currency, amount, destChannel, destProduct, metadata);
            var success = response.Status == "Queued";
            Assert.IsTrue(success);
        }

        // [TestMethod]
        // public void DoBankCheckout()
        // {
        //     const string otp = "1234";
        //     string transId = "id";
        //     var productName = "coolproduct";
        //     var accountName = "Fela Kuti";
        //     var accountNumber = "1234567890";
        //     var bankCode = 234001;
        //     var currencyCode = "NGN";
        //     var amount = 1000.5M;
        //     var dob = "2017-11-22";
        //     var metadata = new Dictionary<string, string> { { "Reason", "Do something cool" } };
        //     var narration = "We're buying something cool";
        //     var receBank = new BankAccount(accountNumber, bankCode, dob, accountName);
        //     var getTransactionId = _atGWInstance.BankCheckout(productName, receBank, currencyCode, amount, narration, metadata);
        //     transId = getTransactionId["transactionId"];
        //     var validateTransaction = _atGWInstance.OtpValidate(transId, otp);
        //     validateTransaction = JsonConvert.DeserializeObject(validateTransaction);
        //     var success = validateTransaction["status"] == "Success";
        //     Assert.IsTrue(success, "Should succcessfully process bank checkout");
        // }
    }
}