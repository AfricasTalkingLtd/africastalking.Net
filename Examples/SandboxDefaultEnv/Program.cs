using AfricasTalkingCS;
using System;

namespace SandboxDefaultEnv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string username = "sandbox";
            const string apikey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            const string phoneNum = "+254724587654";
            const string message = "I have been Authe'd by default";
            var gateway = new AfricasTalkingGateway(username, apikey);
            Console.WriteLine("Hello World! Testing Default Auth");
            try
            {
                var sms = gateway.SendMessage(phoneNum, message);
                Console.WriteLine(sms);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}