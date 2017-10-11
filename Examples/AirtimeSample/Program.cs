using System;
using  AfricasTalkingCS;

namespace AirtimeSample
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "sandbox";
            var apikey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            var airtimerecipients = @"{'phoneNumber':'+254724587654','amount':'KES 250'}"; // Send any JSON object
            Console.WriteLine(airtimerecipients);
            Console.ReadLine();
            var env = "sandbox";
            var airtimeGateway = new AfricasTalkingGateway(username, apikey, env);
            try
            {
                dynamic airtimeTransaction = airtimeGateway.SendAirtime(airtimerecipients);
                Console.WriteLine(airtimeTransaction);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into issues: " + e.StackTrace + ": " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
