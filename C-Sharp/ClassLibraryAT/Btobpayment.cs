
using System;

namespace ATpayment
{
    class Btobpayment
    {
        static public void B2tobpayment()
        {
            //Specify your credentials
            string username        = "MyAfricasTalkingUsername";
            string apiKey          = "MyAfricasTalkingAPIKey";

            //Create an instance of our awesome gateway class and pass your credentials

            // Specify the name of your Africa's Talking payment product
            string productName     = "myPaymentProductName";
            // The phone number of the customer checking out
            string provider        = "myPaymentProvider";
            // Transfer Type
            string transferType    = "transferType";
            // The 3-Letter ISO currency code for the b2b amount
            string currencyCode    = "KES"; 
            // The amount
            int amount             = 100;
            // The destination Channel - Optional
            string destinationChannel = "partnerBusinessChannel";
            // The destination Account
            string destinationAccount = "partnerBusinessAccount"; 

            // Create a new instance of our awesome gateway class
            AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey);
             
            try
            {
                // Initiate the b2b request. If successful, you will get back a json response

                dynamic b2bResponse = gateway.MobileB2B(productName, provider, transferType, currencyCode,amount, destinationChannel, destinationAccount);

                Console.WriteLine(b2bResponse);

            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Encountered an error: " + e.Message);
            }
            Console.Read();
        }
    }
}
