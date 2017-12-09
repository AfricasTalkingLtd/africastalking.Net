namespace ValidateCardOTPOnly
{
    using System;
    using Newtonsoft.Json;
    using AfricasTalkingCS;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string Otp = "1234";
            const string ApiKey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            const string TransactionId = "ATPid_55bb5be3130b1a282143257f0f15f1cd";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, ApiKey, Env);
            try
            {
                var validate = gateway.ValidateCardOtp(TransactionId, Otp);
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
