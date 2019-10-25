using AfricasTalkingCS;
using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var username = "sandbox";
            var environment = "sandbox";
            var apiKey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            var productName = "coolproduct";
            var phoneNumber = "+254724587654";
            var currency = "KES";
            var amount = 35700;
            var channel = "mychannel";
            var metadata = new Dictionary<string, string>
            {
                {"reason", "broken car"}
            };

            var gw = new AfricasTalkingGateway(username, apiKey, environment);

            try
            {
                var checkout = gw.Checkout(productName, phoneNumber, currency, amount, channel, metadata);
                Console.WriteLine(checkout);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}