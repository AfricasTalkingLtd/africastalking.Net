using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using  AfricasTalkingCS;
using Newtonsoft.Json.Linq;

namespace SampleB2B
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Suppose we want to move money from our *awesomeproduct* to *coolproduct*
            /*
             * Remember to register  B2B products and callback urls:else these trasactions will fail
             */
            string username = "sandbox";
            string environment = "sandbox";
            string apiKey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            string productName = "awesomeproduct";
            string currencyCode = "KES";
            decimal amount = 15020M;
            string provider = "Athena";
            string destinationChannel = "mychannel"; //This is the channel that will be receiving the payment
            string destinationAccount = "coolproduct";
            //Transfer Type
            /*
             *BusinessBuyGoods
             *BusinessPayBill
             *DisburseFundsToBusiness
             *BusinessToBusinessTransfer
             */
            dynamic metadataDetails =  new JObject();
            metadataDetails.shopName = "cartNumber";
            metadataDetails.cartId = "50";
            string metadata = metadataDetails.ToString();
            metadata = metadata.TrimEnd('\r', '\n');
            string transferType = "BusinessToBusinessTransfer";
            AfricasTalkingGateway gateWay = new AfricasTalkingGateway(username,apiKey,environment);
           
            
            try
            {
                string response = gateWay.MobileB2B(productName, provider, transferType, currencyCode, amount,
                    destinationChannel, destinationAccount, metadata);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Woopsies! We ran into issues: "+e.StackTrace+" : "+e.Message);
               
            }
            Console.ReadLine();
        }
    }
}
