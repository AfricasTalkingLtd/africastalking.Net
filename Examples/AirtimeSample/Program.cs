using System;
using  AfricasTalkingCS;

namespace AirtimeSample
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class AirtimeUsers {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "Username";
            var apikey = "ABCXYZ";
            var airtimeUser = new AirtimeUsers();
            airtimeUser.PhoneNumber = "+254XEPPPPPPP";
            airtimeUser.Amount = "KES 10";
            var airtimeRec = JsonConvert.SerializeObject(airtimeUser);
            var airtimeGateway = new AfricasTalkingGateway(username, apikey);
            try
            {
                dynamic airtimeTransaction = airtimeGateway.SendAirtime(airtimeRec);
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
