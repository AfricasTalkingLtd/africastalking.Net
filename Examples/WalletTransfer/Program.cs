using System;

namespace WalletTransfer
{
    using AfricasTalkingCS;

    using System;
    using System.Collections.Generic;
    class Program
    {
        static void Main(string[] args)
        {
            const string username    = "sandbox";
            const string apikey      = "1f6c70805d287caf585e4062f0f2abeccc1d067ce7a36b28038d5057c823c727";
            const int productCode    = 1234;
            const string productName = "coolproduct";
            decimal amount           = 150M;
            string currencyCode      = "KES";
            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                {"mode" , "transfer"}
            };

            var gw = new AfricasTalkingGateway(username, apikey);

            try
            {
                
                Console.WriteLine("Hello World!");
                var res = gw.WalletTransfer(productName, productCode, currencyCode, amount, metadata);
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
                
                Console.WriteLine("We had an errror: " + e);
            }

            Console.ReadLine();
        }
    }
}
