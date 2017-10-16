using System;
using AfricasTalkingCS;
namespace CreateSubscription
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apikey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var shortCode = "44005";
            var keyword = "coolguy";
            var phoneNum = "+254724587654";
            var token = "CkTkn_6d1d6387-ff82-45bd-9ceb-d125097d93c8";
            try
            {
                var response = gateway.CreateSubscription(phoneNum, shortCode, keyword, token);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We hit a snag: " + e.StackTrace + ". " + e.Message);
                throw;
            }

            Console.ReadLine();
        }
    }
}
