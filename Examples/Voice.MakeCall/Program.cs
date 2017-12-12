using System;
using AfricasTalkingCS;
namespace Voice.MakeCall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apiKey = "Key";
            var env = "sandbox";
            var from = "+254724545678";
            var to = "+254724587654";
            var gateway = new AfricasTalkingGateway(username, apiKey, env);

            try
            {
                var results = gateway.Call(from, to);
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
