using AfricasTalkingCS;
using Newtonsoft.Json;
using System;

namespace AirtimeSample
{
    internal class AirtimeUsers
    {
        [JsonProperty("phoneNumber")] public string PhoneNumber { get; set; }

        [JsonProperty("amount")] public string Amount { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "Username";
            var apikey = "ABCXYZ";
            var airtimeUser = new AirtimeUsers { PhoneNumber = "+254XEPPPPPPP", Amount = "KES 10" };
            var airtimeRec = JsonConvert.SerializeObject(airtimeUser);
            var airtimeGateway = new AfricasTalkingGateway(username, apikey);
            try
            {
                var airtimeTransaction = airtimeGateway.SendAirtime(airtimeRec);
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