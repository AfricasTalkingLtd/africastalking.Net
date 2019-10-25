using System;
using AfricasTalkingCS;

namespace CreateSubscription
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apikey = "KEY";
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var shortCode = "NNNNN";
            var keyword = "keyword";
            var phoneNum = "+254XXXXXXXXX";
            var token = "Token";
            try
            {
                var response = gateway.CreateSubscription(phoneNum, shortCode, keyword, token);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We hit a snag: " + e.StackTrace + ". " + e.Message);
                throw;
            }

            Console.ReadLine();
        }
    }
}