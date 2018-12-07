using System;

namespace TopupStash
{
    using AfricasTalkingCS;

    using System;
    using System.Collections.Generic;
    class Program
    {
        static void Main(string[] args)
        {
            const string username    = "sandbox";
            const string apikey      = "54a533717fcb1be4577e96ea7a02c1ca9c6e802a56a670379085582ca3c118a7";
            const string productName = "coolproduct";
            decimal amount           = 150M;
            string currencyCode      = "KES";
            Dictionary <string, string> metadata = new Dictionary<string, string> {
                {"what this is","cool stuff"}
            };

                        var gw = new AfricasTalkingGateway(username, apikey);

            try
            {
                
                Console.WriteLine("Hello World!");
                StashResponse res = gw.TopupStash(productName, currencyCode, amount, metadata);
                Console.WriteLine(res.ToString());
            }
            catch (AfricasTalkingGatewayException e)
            {
                
                Console.WriteLine("We had an errror: " + e);
            }

            Console.ReadLine();
        }
    }
}
