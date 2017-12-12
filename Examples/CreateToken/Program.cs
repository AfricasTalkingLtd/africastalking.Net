
namespace CreateToken
{
    using System;
    using  AfricasTalkingCS;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var phoneNumber = "+254724587654";
            var gateway = new AfricasTalkingGateway("username", "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745", "sandbox");
            try
            {
                var token = gateway.CreateCheckoutToken(phoneNumber);
                Console.WriteLine("Your Token is:  " + token);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
