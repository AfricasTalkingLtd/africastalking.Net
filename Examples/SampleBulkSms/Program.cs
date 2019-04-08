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
            var apiKey = "43aa714eb2dab0fab27466563a1c051464e94eb42068dca2b0da31d1b61bbf58";
            var recep = "+254720000000,+254720000001";
            var msg = "Super awesome message ☻ 😁";
               

            var gateway = new AfricasTalkingGateway(username, apiKey);
            try
            {
                dynamic res = gateway.SendMessage(recep, msg);
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
