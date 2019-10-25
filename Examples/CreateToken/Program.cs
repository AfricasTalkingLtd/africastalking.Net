using AfricasTalkingCS;
using System;

namespace CreateToken
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var phoneNumber = "+254724587654";
            var gateway = new AfricasTalkingGateway("username", "Key", "sandbox");
            try
            {
                var token = gateway.CreateCheckoutToken(phoneNumber);
                Console.WriteLine("Your Token is:  " + token["token"]);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}