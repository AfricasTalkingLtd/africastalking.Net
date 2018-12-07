using System;
using AfricasTalkingCS;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FindTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "sandbox";
            const string apikey  = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";

            AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apikey);
            var phoneNumber = "+254720000001";
            var productName = "coolproduct";
            var currency = "KES";
            decimal amount = 1000M;
            var providerChannel = "mychannel";
            var metadata = new Dictionary<string, string>
                {
                    {"dest","oracle"}
                };
            try
            {
            // Example only | Use older transactions, the results here will be "Failure"
            C2BDataResults checkoutResponse = gateway.Checkout(productName, phoneNumber, currency, amount, providerChannel, metadata);
            var transactionId = checkoutResponse.TransactionId;
            var findId = gateway.FindTransaction(transactionId);
            JObject findIdObject = JObject.Parse(findId);
            Console.WriteLine(findIdObject);
            }
            catch (AfricasTalkingGatewayException e)
            {
                
                throw new AfricasTalkingGatewayException(e.Message);
            }
        }
    }
}
