namespace USSDPushSample
{
    using System;
    using AfricasTalkingCS;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string Apikey = "Key";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username, Apikey, Env);
            var tokenId = "CkTkn_94248929024020408fh3hf02302qawjlasj32";
            const string PhoneNumber = "+254724587654";
            const string Menu = "CON You're about to love C#\n1.Accept my fate\n2.No Never\n";

            // Let's create a checkout token  first
            try
            {
                var tkn = gateway.CreateCheckoutToken(PhoneNumber);
                if (tkn["description"] == "Success")
                {
                    tokenId = tkn["token"];
                }

                // Then send user menu...
                var prompt = gateway.InitiateUssdPushRequest(PhoneNumber, Menu, tokenId);
                if (prompt["errorMessage"] == "None")
                {
                    Console.WriteLine("Awesome");
                }
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Woopsies : " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
