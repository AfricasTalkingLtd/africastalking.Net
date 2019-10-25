using AfricasTalkingCS;
using System;
using System.Collections;

namespace SendPremiumSMS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apikey = "KEY";
            var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var opts = new Hashtable { ["keyword"] = "mykeyword" }; // ....
            var from = "NNNNN";
            var to = "+2547XXXXX";
            var message = "Super Cool Message";
            try
            {
                var res = gateway.SendMessage(to, message, from, 1, opts); // Set Bulk to true
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Whoops: " + e.Message);
                throw;
            }
        }
    }
}