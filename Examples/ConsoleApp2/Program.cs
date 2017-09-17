using System;
using  AfricasTalkingCS;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "sandbox";
            string environment = "sandbox";
            string apiKey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            string productName = "coolproduct";
            string phoneNumber = "+254724587654";
            string currency = "KES";
            int amount = 35700;
            string channel = "mychannel";

            var gw = new AfricasTalkingGateway(username,apiKey,environment);

            try
            {
               
                var checkout = gw.Checkout(productName, phoneNumber, currency, amount, channel);
                Console.WriteLine(checkout);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: "+ e.Message);
                
            }
            Console.ReadLine();

        }
    }
}
