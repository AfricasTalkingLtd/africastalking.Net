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
            string username = "KennedyOtieno";
            string apiKey = "9f072129e0b34163eb8460a42f537bbb1564f50f8ca069fa779e27c64171f4e1";
            string productName = "iot";
            string currencyCode = "KES";
            decimal amount = 15;
            string provider = "MPESA";
            string destinationChannel = "525900"; //This is the channel that will be receiving the payment
            string destinationAccount = "kennedyotieno.api";
            //Transfer Type
            /*
             *BusinessBuyGoods
             *BusinessPayBill
             *DisburseFundsToBusiness
             *BusinessToBusinessTransfer
             */
            dynamic metadataDetails =  new JObject();
            metadataDetails.shopName = "cartNumber";
            metadataDetails.Info = "Stuff";
            string transferType = "BusinessToBusinessTransfer";
            var gateWay = new AfricasTalkingGateway(username,apiKey);
           
            
            try
            {
                string response = gateWay.MobileB2B(productName, provider, transferType, currencyCode, amount,
                    destinationChannel, destinationAccount, metadataDetails);
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
