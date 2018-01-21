using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfricasTalkingCS;

namespace SampleBulkSms
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = "sandbox";
            var apiKey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            var recep = "+254724587654,+254714587654,+254704876545";
            var msg = "Super awesome message ☻ 😁";
               

            var gateway = new AfricasTalkingGateway(username, apiKey);
            try
            {
                dynamic res = gateway.SendMessage(recep, msg);
                foreach (var re in res["SMSMessageData"]["Recipients"])
                {
                    Console.WriteLine((string)re["number"] + ": ");
                    Console.WriteLine((string)re["status"] + ": ");
                    Console.WriteLine((string)re["messageId"] + ": ");
                    Console.WriteLine((string)re["cost"] + ": ");
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
