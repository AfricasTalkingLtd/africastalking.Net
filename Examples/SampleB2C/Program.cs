using AfricasTalkingCS;
using System;
using System.Collections.Generic;

namespace SampleB2C
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Suppose a superhero unknowingly paid for a suit they did not like,and we want to refund them
            // or you want to pay your employees,staff etc...
            // Note::
            /*
             * Remember: In a live account ensure you have registered a credible B2C product and a callback url else these transactions will fail
             */

            // Developer Details
            var username = "sandbox";
            //string environment = "sandbox";
            var apiKey = "APIKEY";
            var productName = "coolproduct";
            var currencyCode = "KES";

            // Recepient details,these can be retrieved from a db..or somewhere else then parsed...we'll keep it simple
            var hero1PhoneNum = "+254714978532";
            var hero2PhoneNum = "+254712387452";
            var hero1Name = "Batman";
            var hero2Name = "Superman";
            var hero1amount = 15M;
            var hero2amount = 54000M;

            // We invoke our gateway
            var gateway = new AfricasTalkingGateway(username, apiKey);

            // Let's create a bunch of people who'll be receiving the refund or monthly salary etc...
            var hero1 = new MobileB2CRecepient(hero1Name, hero1PhoneNum, currencyCode, hero1amount);

            // we can add metadata...like why we're paying them/refunding them etc...
            hero1.AddMetadata("reason", "Torn Suit");
            var hero2 = new MobileB2CRecepient(hero2Name, hero2PhoneNum, currencyCode, hero2amount);
            hero2.AddMetadata("reason", "Itchy Suit");

            // ....etc

            // Next we create a recepients list
            IList<MobileB2CRecepient> heroes = new List<MobileB2CRecepient>
            {
                hero1,
                hero2
            };

            // then refund them so that we don't get into trouble
            try
            {
                var response = gateway.MobileB2C(productName, heroes);
                Console.WriteLine(heroes);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: " + e.StackTrace + e.Message);
            }

            Console.ReadLine();
        }
    }
}