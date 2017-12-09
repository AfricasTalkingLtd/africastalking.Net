using System;
using AfricasTalkingCS;

namespace OTPCardValidation
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string Otp = "1234";
            const string ApiKey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            var transactionId = "id";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, ApiKey, Env);
            const string ProductName = "coolproduct";
            const string CurrencyCode = "NGN";
            const decimal Amount = 50000M;
            const string Narration = "Buy Aluku Records";
            var metadata = new Dictionary<string, string>
                               {
                                   { "Parent Company",  "Offering Records" },
                                   { "C.E.O", "Boddhi Satva" }
                               };
            const short CardCvv = 654;
            const string CardNum = "123456789000";
            const string CountryCode = "NG";
            const string CardPin = "2345";
            const string ValidTillMonth = "12";
            const string ValidTillYear = "2019";
            var cardDetails = new CardDetails(CardNum, CountryCode, CardCvv, ValidTillMonth, ValidTillYear, CardPin);

            try
            {
                // 1. Perform a card Checkout, recive the Tranasaction ID then,
                // 2. validate against this OTP
                var checkout = gateway.CardCheckout(
                    ProductName,
                    cardDetails,
                    CurrencyCode,
                    Amount,
                    Narration,
                    metadata);
                /** Expect
                 * {
                    "status": "PendingValidation",
                    "description": "Waiting for user input",
                    "transactionId": "ATPid_SampleTxnId123"
                    }
                 * 
                 */
                var resObject = JsonConvert.DeserializeObject(checkout);
                if (resObject["status"] == "PendingValidation")
                {
                    transactionId = resObject["transactionId"];
                }
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We encountred issues : " + e.Message + e.StackTrace);
                throw;
            }

            Console.WriteLine("Attempting to Validate");
            
            try
            {
                var validate = gateway.ValidateCardOtp(transactionId, Otp);
                var res = JsonConvert.DeserializeObject(validate);
                if (res["status"] == "Success")
                {
                    Console.WriteLine("Awesome");
                }
                else
                {
                    Console.WriteLine("We had an error " + res["status"]);
                }
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Validation Error occured : " + e.Message);
                throw;
            }

            Console.ReadLine();
        }
    }
}
