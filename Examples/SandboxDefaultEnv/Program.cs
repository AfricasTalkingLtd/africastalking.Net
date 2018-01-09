using System;
using AfricasTalkingCS;
namespace SandboxDefaultEnv
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "sandbox";
            const string apikey = "mydopeSandboxKey";
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
