using System;
using  AfricasTalkingCS;
namespace GetUser_Balance
{
    using System.Xml.Schema;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var username = "yourLIVEAccounntUsername";
            var apiKey = "yourLIVEAccountAPIKEY";
            
            var gw = new AfricasTalkingGateway(username, apiKey);

            try
            {
                dynamic res = gw.GetUserData();
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
