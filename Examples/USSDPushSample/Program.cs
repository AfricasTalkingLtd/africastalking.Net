using System;
using AfricasTalkingCS;

namespace USSDPushSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string Apikey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, Apikey, Env);
            var tokenId = "CkTkn_94248929024020408fh3hf02302qawjlasj32";
            const string PhoneNumber = "+2348092226042";
            const string Menu = "CON You're about to love C#\n1.Accept my fate\n2.No Never\n";

            // Let's create a checkout token  first
            try
            {
                var tkn = gateway.CreateCheckoutToken(PhoneNumber);
                if (tkn["description"] == "Success") tokenId = tkn["token"];

                // Then send user menu...
                var prompt = gateway.InitiateUssdPushRequest(PhoneNumber, Menu, tokenId);
                if (prompt["errorMessage"] == "None") Console.WriteLine("Awesome");
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Woopsies : " + e.Message);
            }

            Console.ReadLine();
        }
    }
}