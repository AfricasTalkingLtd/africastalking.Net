using System;
using AfricasTalkingCS;
namespace Voice.MakeCall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "KennedyOtieno";
            var apiKey = "c00d2303f9113955592f78754cf48c6fa861c7c2b9af86aaca0b831d5d829631";
            var from = "+254711082518";
            var to = "+254718101532,+254724825788,+254722477767";
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
