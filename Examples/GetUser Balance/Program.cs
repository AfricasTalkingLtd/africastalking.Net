using AfricasTalkingCS;
using System;

namespace GetUser_Balance
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "yourLIVEAccounntUsername";
            var apiKey = "yourLIVEAccountAPIKEY";

            var gw = new AfricasTalkingGateway(username, apiKey);

            try
            {
                var res = gw.GetUserData();
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into a bunch of problems: " + e.Message + " :" + e.StackTrace);
            }

            Console.ReadLine();
        }
    }
}