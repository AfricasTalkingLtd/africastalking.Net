namespace BankCheckout
{
    using System;
    using System.Collections.Generic;

    using AfricasTalkingCS;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string ApiKey = "Key";
            const string Otp = "1234";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, ApiKey, Env);
            string transId = "id";
            var productName = "coolproduct";
            var accountName = "Fela Kuti";
            var accountNumber = "1234567890";
            var bankCode = 234001;
            var currencyCode = "NGN";
            var amount = 1000.5M;
            var dob = "2017-11-22";
            var metadata = new Dictionary<string, string> { { "Reason", "Buy Vega Records" } };
            var narration = "We're buying something cool";
            var receBank = new BankAccount(accountNumber, bankCode, dob, accountName);
            try
            {
                var res = gateway.BankCheckout(productName, receBank, currencyCode, amount, narration, metadata);
                res = JsonConvert.DeserializeObject(res);
                Console.WriteLine(res);
                if (res["status"] == "PendingValidation")
                {
                    transId = res["transactionId"];
                    Console.WriteLine("Validating...");
                }

                try
                {
                    var valid = gateway.OtpValidate(transId, Otp);
                    valid = JsonConvert.DeserializeObject(valid);
                    if (valid["status"] == "Success")
                    {
                        Console.WriteLine("Whoooohoo...");
                    }
                }
                catch (AfricasTalkingGatewayException e)
                {
                    Console.WriteLine("Yikes: " + e.Message + e.StackTrace);
                }

            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Something odd happened: " + e.Message + e.StackTrace);
            }
            Console.ReadLine();
        }
    }
}
