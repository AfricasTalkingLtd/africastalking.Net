using System;
using AfricasTalkingCS;
namespace Voice.MakeCall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "Username";
            var apiKey = "APIKEY....";
            var from = "+2ABXXYYYYYY";
            var to = "+254720000000,+25472000000, test.mysip@ke.sip.africastalking.com";
            // Optional Param
            var id = "Test";

            var gateway = new AfricasTalkingGateway(username, apiKey);

            try
            {
                var results = gateway.Call(from, to, id);
                Console.WriteLine(results);
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine("Something went horribly wrong: " + exception.Message + ".\nCaused by :" + exception.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
