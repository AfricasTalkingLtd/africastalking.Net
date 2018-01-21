using System;
using AfricasTalkingCS;
namespace FetchMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Username = "sandbox";
            const string ApiKey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            const string Env = "sandbox";
            var gateway = new AfricasTalkingGateway(Username,ApiKey);
            int msgId = 0;
            try
            {
                var res = gateway.FetchMessages(0);
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
               Console.WriteLine("We had an Error: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
