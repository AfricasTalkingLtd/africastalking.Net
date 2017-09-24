using System;
using System.Collections.Generic;
using  AfricasTalkingCS;

namespace SampleB2C
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Suppose a superhero unknowingly paid for a suit they did not like,and we want to refund them
            //or you want to pay your employees,staff etc...
            //Note::
            /*
             * Remember: In a live account ensure you have registered a credible B2C product and a callback url else these transactions will fail
             */
            //Developer Details
            string username = "sandbox";
            string environment = "sandbox";
            string apiKey = "bc203009d2b240e461c22d7a959ca4d752591d4553295d991e74824d599fc9b3";
            string productName = "coolproduct";
            string currencyCode = "KES";

            //Recepient details,these can be retrieved from a db..or somewhere else then parsed...we'll keep it simple
            string hero1PhoneNum = "+254724587654";
            string hero2PhoneNum = "+254712387452";
            string hero1Name = "Batman";
            string hero2Name = "Superman";
            decimal hero1amount = 15000M;
            decimal hero2amount = 54000M;

            //We invoke our gateway
            var gateway = new AfricasTalkingGateway(username,apiKey,environment);

            //Let's create a bunch of people who'll be receiving the refund or monthly salary etc...
            var hero1 = new MobileB2CRecepient(hero1Name,hero1PhoneNum,currencyCode,hero1amount);
            //we can add metadata...like why we're paying them/refunding them etc...
            hero1.AddMetadata("reason","Torn Suit");
            var hero2 = new MobileB2CRecepient(hero2Name,hero2PhoneNum,currencyCode,hero2amount);
            hero2.AddMetadata("reason", "Itchy Suit");
            //....etc

            //Next we create a recepients list
            IList<MobileB2CRecepient> heroes = new List<MobileB2CRecepient>
            {
                hero1,
                hero2
            };

            //then refund them so that we don't get into trouble
            try
            {
                var response = gateway.MobileB2C(productName, heroes);
                Console.WriteLine(heroes);
                Console.WriteLine(response);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We ran into problems: "+e.StackTrace+e.Message);
               
            }
            Console.ReadLine();
        }
    }
}
