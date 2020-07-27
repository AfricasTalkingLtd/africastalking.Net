using Microsoft.VisualStudio.TestTools.UnitTesting;
using AfricasTalkingCS;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace AfricasTalkingCS_Tests
{
    [TestClass]
    public class PaymentService
    {
        private static string apikey = "6c36e56b86c24c2ff66adaff340d60793dff71ac304bc551f7056ca76dd8032a";
        private static string username = "sandbox";
        private readonly AfricasTalkingGateway _atGWInstance = new AfricasTalkingGateway(username,apikey);

        private string TestId = "ATPid_e738bcb66505a9c0cf00868e569e9026";

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
            C2BDataResults checkoutResponse = _atGWInstance.Checkout(productName, phoneNumber, currency, amount, providerChannel, metadata);
            var success = checkoutResponse.Status =="PendingConfirmation";
            TestId = checkoutResponse.TransactionId;
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

        [TestMethod]
        public void DoBankCheckout()
        {
            const string otp = "1234";
            string transId = "id";
            var productName = "coolproduct";
            var accountName = "Fela Kuti";
            var accountNumber = "1234567890";
            var bankCode = 234001;
            var currencyCode = "NGN";
            var amount = 1000.5M;
            var dob = "2017-11-22";
            var metadata = new Dictionary<string, string> { { "Reason", "Do something cool" } };
            var narration = "We're buying something cool";
            var receBank = new BankAccount(accountNumber, bankCode, dob, accountName);
            BankCheckoutResponse getTransactionId = _atGWInstance.BankCheckout(productName, receBank, currencyCode, amount, narration, metadata);
            transId = getTransactionId.TransactionId;
            var validateTransaction = _atGWInstance.OtpValidate(transId, otp);
            validateTransaction = JsonConvert.DeserializeObject(validateTransaction);
            var success = validateTransaction["status"] == "Success";
            Assert.IsTrue(success, "Should succcessfully process bank checkout");
        }

        [TestMethod]
        public void DoBankTransfer()
        {
            const string productname = "coolproduct";
            var currency_code = "NGN";
            var recipient1_account_name = "Alyssa Hacker";
            var recipient1_account_number = "1234567890";
            var recipient1_bank_code = 234001;
            decimal recipient1_amount = 1500.50M;
            var recipient1_narration = "December Bonus";
            var recipient2_account_name = "Ben BitDiddle";
            var recipient2_account_number = "234567891";
            var recipient2_bank_code = 234004;
            decimal recipient2_amount = 1500.50M;
            var recipient2_narration = "November Bonus";
            var recepient1_account = new BankAccount(recipient1_account_number, recipient1_bank_code, recipient1_account_name);
            var recepient1 = new BankTransferRecipients(recipient1_amount, recepient1_account, currency_code, recipient1_narration);
            recepient1.AddMetadata("Reason", "Early Bonus");
            var recipient2_account = new BankAccount(recipient2_account_number, recipient2_bank_code, recipient2_account_name);
            var recipient2 = new BankTransferRecipients(recipient2_amount, recipient2_account, currency_code, recipient2_narration);
            recipient2.AddMetadata("Reason", "Big Wins");
            IList<BankTransferRecipients> recipients = new List<BankTransferRecipients>
                                                           {
                                                               recepient1,
                                                               recipient2
                                                           };
            BankTransferResults bankTransferResults = _atGWInstance.BankTransfer(productname, recipients);
            var success = (bankTransferResults.Entries.Count == 2 ); // Number of recipinets is 2.
            Assert.IsTrue(success, "Should succesfully transfer monies between bank accounts");
        }

        [TestMethod]
        public void DoCardCheckout()
        {
            const string Otp = "1234";
            var transactionId = "id";
            const string ProductName = "awesomeproduct";
            const string CurrencyCode = "NGN";
            const decimal Amount = 7500.50M;
            const string Narration = "Buy Aluku Records";
            var metadata = new Dictionary<string, string>
                               {
                                   { "Parent Company",  "Offering Records" },
                                   { "C.E.O", "Boddhi Satva" }
                               };
            const short CardCvv = 123;
            const string CardNum = "123456789012345";
            const string CountryCode = "NG";
            const string CardPin = "1234";
            const int ValidTillMonth = 9;
            const int ValidTillYear = 2019;
            var cardDetails = new PaymentCard(CardPin, CountryCode, CardCvv, ValidTillMonth, ValidTillYear, CardNum);
            CardCheckoutResults checkout = _atGWInstance.CardCheckout(
                    ProductName,
                    cardDetails,
                    CurrencyCode,
                    Amount,
                    Narration,
                    metadata);
            transactionId = checkout.TransactionId;
            var validate = _atGWInstance.ValidateCardOtp(transactionId, Otp);
            var res = JsonConvert.DeserializeObject(validate);
            var success = res["status"] == "Success" && checkout.Status == "PendingValidation";
            Assert.IsTrue(success, "Should succesfully complete a Card checkout transaction");
        }

        [TestMethod]
        public void DoWalletTransfer()
        {
            const int productCode    = 1234;
            const string productName = "coolproduct";
            decimal amount           = 150M;
            string currencyCode      = "KES";
            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                {"mode" , "transfer"}
            };

            StashResponse stashResponse = _atGWInstance.WalletTransfer(productName, productCode, currencyCode, amount, metadata);
            var success = stashResponse.Status == "Success";
            Assert.IsTrue(success, "Should transfer amounts between wallets");
        }
        
        [TestMethod]
        public void DoTopupStash()
        {
            const string productName = "coolproduct";
            decimal amount           = 150M;
            string currencyCode      = "KES";
            Dictionary <string, string> metadata = new Dictionary<string, string> {
                {"what this is","cool stuff"}
            };
            StashResponse stashResponse = _atGWInstance.TopupStash(productName, currencyCode, amount, metadata);
            var success = stashResponse.Status == "Success";
            Assert.IsTrue(success, "Should successfully topup product stash");
        }

        [TestMethod]
        public void DoFindTransaction()
        {
            string transactionIdResponse = _atGWInstance.FindTransaction(TestId);
            JObject transactionIdResponseJson = JObject.Parse(transactionIdResponse);
            var status = transactionIdResponseJson.GetValue("status");
            var success = (status.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully find a transaction given it's ID");
        }

        [TestMethod]
        public void DoFetchProductTransactions()
        {
            const string productName = "coolproduct";
            const string pageNumber = "1";
            const string count = "3";
            string fetchTransactionsResponse = _atGWInstance.FetchProductTransactions(productName, pageNumber,count);
            JObject fetchTransactionsResponseJson = JObject.Parse(fetchTransactionsResponse);
            var fetchTransactionsResponseStatus = fetchTransactionsResponseJson.GetValue("status");
            var success = (fetchTransactionsResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch transactions with default params");
        }

        [TestMethod] 
        public void DoFetchProductTransactionsByDateDate() 
        {
            const string productName = "coolproduct";
            const string pageNumber = "1";
            const string count = "3";
            DateTime today = DateTime.Today;
            string startDate = today.ToString("yyyy-MM-dd");
            string endDate = today.ToString("yyyy-MM-dd");
            string fetchTransactionsResponse = _atGWInstance.FetchProductTransactions(productName, pageNumber,count, startDate, endDate);
            JObject fetchTransactionsResponseJson = JObject.Parse(fetchTransactionsResponse);
            var fetchTransactionsResponseStatus = fetchTransactionsResponseJson.GetValue("status");
            var success = (fetchTransactionsResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch transactions based on given date");
        }

        [TestMethod]
        public void DoFetchProductTransactionByCategory()
        {
            const string productName = "coolproduct";
            const string pageNumber = "1";
            const string count = "3";
            const string category = "MobileCheckout";
            string fetchTransactionsResponse = _atGWInstance.FetchProductTransactions(productName, pageNumber,count, category);
            JObject fetchTransactionsResponseJson = JObject.Parse(fetchTransactionsResponse);
            var fetchTransactionsResponseStatus = fetchTransactionsResponseJson.GetValue("status");
            var success = (fetchTransactionsResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch transactions based on category");
        }

        [TestMethod]
        public void DoFetchWalletTransactions()
        {
            const string pageNumber = "1";
            const string count = "3";
            string fetchTransactionsResponse = _atGWInstance.FetchWalletTransactions(pageNumber,count);
            JObject fetchTransactionsResponseJson = JObject.Parse(fetchTransactionsResponse);
            var fetchTransactionsResponseStatus = fetchTransactionsResponseJson.GetValue("status");
            var success = (fetchTransactionsResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch wallet transactions with default params");
        }

        [TestMethod]
        public void DoFetchWalletTransactionsByDate()
        {
            const string pageNumber = "1";
            const string count = "3";
            DateTime today = DateTime.Today;
            string startDate = today.ToString("yyyy-MM-dd");
            string endDate = today.ToString("yyyy-MM-dd");
            string fetchTransactionsResponse = _atGWInstance.FetchWalletTransactions(pageNumber,count, startDate, endDate);
            JObject fetchTransactionsResponseJson = JObject.Parse(fetchTransactionsResponse);
            var fetchTransactionsResponseStatus = fetchTransactionsResponseJson.GetValue("status");
            var success = (fetchTransactionsResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch wallet transactions by date");
        }

        [TestMethod]
        public void DoFetchWalletBalance()
        {
            string fetchBalanceResponse = _atGWInstance.FetchWalletBalance();
            JObject fetchBalanceResponseJson = JObject.Parse(fetchBalanceResponse);
            var fetchBalanceResponseStatus = fetchBalanceResponseJson.GetValue("status");
            var success = (fetchBalanceResponseStatus.ToString() == "Success");
            Assert.IsTrue(success, "Should successfully fetch wallet balance");
        }
    }
}