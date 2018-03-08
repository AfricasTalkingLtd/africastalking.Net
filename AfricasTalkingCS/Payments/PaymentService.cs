using AfricasTalkingSDK.Payments;
using AfricasTalkingSDK.Payments.Response;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class PaymentService : Service
    {
        private object _instance;

        public PaymentService(string username, string apiKey) : base(username, apiKey, "payments", "payments") { }
        protected override dynamic GetInstance(string username, string apiKey)
        {
            if (_instance == null)
            {
                _instance = new PaymentService(username, apiKey);
            }
            return _instance;
        }

        public CheckoutResponse CardCheckout(string productName, string amount, string paymentCard, string narration, Hashtable metadata)
        {
            var requestData = PrepareCheckoutRequest(productName, amount, narration, metadata);
            requestData.Add("paymentCard", paymentCard.ToString());
            var response = MakeRequest("card/checkout/charge", "POST", requestData);
            CheckoutResponse checkoutResponse = JsonConvert.DeserializeObject<CheckoutResponse>(response);
            return checkoutResponse;
        }

        public CheckoutResponse BankCheckout(string productName, string amount, BankAccount bankAccount, string narration, Hashtable metadata)
        {
            var requestData = PrepareCheckoutRequest(productName, amount, narration, metadata);
            requestData.Add("bankAccount", bankAccount.ToString());
            var response = MakeRequest("bank/checkout/charge", "POST", requestData);
            CheckoutResponse checkoutResponse = JsonConvert.DeserializeObject<CheckoutResponse>(response);
            return checkoutResponse;
        }

        public BankTransferResponse BankTransfer(string productName, List<Bank> recipients)
        {
            var recipientsStr = "[" + string.Join(",", recipients.ToString()) + "]";
            var requestData = new Dictionary<string, string>
            {
                { "productName", productName },
                { "recipients", recipientsStr }
            };
            var response = MakeRequest("bank/transfer", "POST", requestData);
            BankTransferResponse bankTransferResponse = JsonConvert.DeserializeObject<BankTransferResponse>(response);
            return bankTransferResponse;
        }

        public B2BResponse MobileB2B(string productName, Business business)
        {
            var requestData = new Dictionary<string, string>
            {
                { "productName", productName },
                { "recipients", business.ToString() }
            };
            var response = MakeRequest("mobile/b2b/request", "POST", requestData);
            B2BResponse b2BResponse = JsonConvert.DeserializeObject<B2BResponse>(response);
            return b2BResponse;
        }

        public B2CResponse MobileB2C(string productName, List<Consumer> consumers)
        {
            var consumersStr = "[" + consumers.ToString() + "]";
            var requestData = new Dictionary<string, string>
            {
                { "productName", productName },
                { "recipients", consumersStr }
            };
            var response = MakeRequest("mobile/b2c/request", "POST", requestData);
            B2CResponse b2CResponse = JsonConvert.DeserializeObject<B2CResponse>(response);
            return b2CResponse;
        }

        public CheckoutResponse MobileCheckout(string productName, string phoneNumber, string amount, string narration, Hashtable metadata)
        {
            try
            {
                CheckPhoneNumber(phoneNumber);
                var requestData = PrepareCheckoutRequest(productName, amount, narration, metadata);
                requestData.Add("phoneNumber", phoneNumber);
                var response = MakeRequest("mobile/checkout/request", "POST", requestData);
                CheckoutResponse checkoutResponse = JsonConvert.DeserializeObject<CheckoutResponse>(response);
                return checkoutResponse;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public CheckoutValidateResponse ValidateCardCheckout(string transactionId, string token)
        {
            var requestData = MakeCheckoutValidationRequest(transactionId, token);
            var response = MakeRequest("card/checkout/validate", "POST", requestData);
            CheckoutValidateResponse checkoutValidateResponse = JsonConvert.DeserializeObject<CheckoutValidateResponse>(response);
            return checkoutValidateResponse;
        }

        public CheckoutValidateResponse ValidateBankCheckout(string transactionId, string token)
        {
            var requestData = MakeCheckoutValidationRequest(transactionId, token);
            var response = MakeRequest("bank/checkout/validate", "POST", requestData);
            CheckoutValidateResponse checkoutValidateResponse = JsonConvert.DeserializeObject<CheckoutValidateResponse>(response);
            return checkoutValidateResponse;
        }

        private IDictionary PrepareCheckoutRequest(string product, string amount, string narration, Hashtable metadata)
        {
            var requestData = new Dictionary<string, string>
            {
                { "productName", product }
            };

            if (narration != null)
            {
                requestData.Add("narration", narration);
            }

            if (metadata != null && metadata.Count > 0)
            {
                requestData.Add("metadata", metadata.ToString());
            }
            return requestData;
        }

        private IDictionary MakeCheckoutValidationRequest(string transactionId, string otp)
        {
            var requestData = new Dictionary<string, string>
            {
                { "transactionId", transactionId },
                { "otp", otp }
            };
            return requestData;
        }
    }
}
