using System;
using AfricasTalkingCS;
namespace SendPremiumSMS
{
    using System.Collections;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "USERNAME";
            var apikey = "APIKEY";
            // var env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey/*, env*/);
            var opts = new Hashtable { ["keyword"] = "myKeyword", ["linkId"] = "NNNNNNN" }; // ....
            var from = "NNNNNNNNNN";
            var to = "+NNNNNNNNNN";
            var message = "Super Cool Message";
            try
            {
                var res = gateway.SendPremiumMessage(to, message, from, 0, opts); // Set Bulk to true
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
