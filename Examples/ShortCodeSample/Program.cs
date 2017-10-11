using System;
using AfricasTalkingCS;
namespace ShortCodeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apikey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            var recepient = "+254724587654,+254712852654";
            var from = "44008";
            var message = "We're those guys";
            var env = "sandbox";

            var gateway  = new AfricasTalkingGateway(username,apikey,env);

            try
            {
                dynamic response = gateway.SendMessage(recepient, message, from);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
               Console.WriteLine("Whoopsies: "+ e.Message +"."+e.StackTrace);
            }
            Console.ReadLine();
        }
    }
}
