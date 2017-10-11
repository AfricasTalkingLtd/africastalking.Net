using System;
/*
 * On your project, RIGHT-CLICK and select "Manage Nuget Packages"
 * Search for Africa's Talking and install the LATEST
 * Accept the licences and build your project once installed to restore all dependencies
 */
using AfricasTalkingCS;
using Newtonsoft.Json.Linq;

namespace VoiceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dev Credentials
            var username = "MyUsername"; //use "sandbox for testing"
            var apikey = "MyApiKey"; //use sandbox API key for testing
            var environment = "sandbox"; //do not declare for live

            //Registered Africa's Talking Phone Number
            var caller = "+254-MY-NUMBER";

            //Numbers to call
            var recepients = "+254-ANOTHER,+254-OTHERNMBER";

            //Create an instance of our gateway
            var gateway = new AfricasTalkingGateway(username,apikey,environment);

            try
            {
                dynamic result = gateway.Call(caller, recepients);
                foreach (var i in result)
                {
                    Console.WriteLine(result["status"] + ",");
                    Console.WriteLine(result["phoneNumber"]+"\n");
                }
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine("Encountered an error: " +exception.Message);
                
            }
        }
    }
}
