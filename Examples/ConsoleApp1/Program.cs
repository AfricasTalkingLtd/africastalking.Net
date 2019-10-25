using System;
using AfricasTalkingCS;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            var username = "YourUSERNAME";
            var apiKey = "yourAPIKEY";
            var env = "sandbox";
            var recep = "recipient";
            var msg = "Super awesome message ☻ 😁";


            var gateway = new AfricasTalkingGateway(username, apiKey, env);
            try
            {
                var res = gateway.SendMessage(recep, msg);
                foreach (var re in res["SMSMessageData"]["Recipients"])
                {
                    Console.WriteLine((string) re["number"] + ": ");
                    Console.WriteLine((string) re["status"] + ": ");
                    Console.WriteLine((string) re["messageId"] + ": ");
                    Console.WriteLine((string) re["cost"] + ": ");
                }
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine(exception);
            }

            Console.ReadLine();
        }
    }
}