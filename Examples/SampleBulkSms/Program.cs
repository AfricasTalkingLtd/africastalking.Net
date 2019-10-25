using AfricasTalkingCS;
using System;

namespace SampleBulkSms
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var username = "sandbox";
            var apiKey = "43aa714eb2dab0fab27466563a1c051464e94eb42068dca2b0da31d1b61bbf58";
            var recep = "+254720000000,+254720000001";
            var msg = "Super awesome message ☻ 😁";


            var gateway = new AfricasTalkingGateway(username, apiKey);
            try
            {
                var res = gateway.SendMessage(recep, msg);
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine(exception);
            }

            Console.ReadLine();
        }
    }
}