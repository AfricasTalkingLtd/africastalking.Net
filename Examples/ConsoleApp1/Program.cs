using System;
using  AfricasTalkingCS;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            string username = "sandbox";
            string apiKey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            string env = "sandbox";
            string recep = "+254724587654";
            string msg = "Woohoo!";

            AfricasTalkingGateway gateway = new AfricasTalkingGateway(username,apiKey,env);

            try
            {
                dynamic res = gateway.SendMessage(recep, msg);
                foreach (var re in res)
                {
                    Console.WriteLine((string)re["number"]+": ");
                    Console.WriteLine((string)re["status"] + ": ");
                    Console.WriteLine((string)re["messageId"] + ": ");
                    Console.WriteLine((string)re["cost"] + ": ");
                }
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine(exception);
               // throw;
            }
            Console.ReadLine();
        }
    }
}
