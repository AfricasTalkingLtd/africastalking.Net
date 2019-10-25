using System;
using AfricasTalkingCS;
using Newtonsoft.Json;

namespace ValidateCardOTPOnly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string Otp = "1234";
            const string ApiKey = "Key";
            const string TransactionId = "ATPid_LFDVLSDNLDSFLDSKLKDE39240DSKFLWDFWI29221efvsdw";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, ApiKey, Env);
            try
            {
                var validate = gateway.ValidateCardOtp(TransactionId, Otp);
                var res = JsonConvert.DeserializeObject(validate);
                if (res["status"] == "Success")
                    Console.WriteLine("Awesome");
                else
                    Console.WriteLine("We had an error " + res["status"]);
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