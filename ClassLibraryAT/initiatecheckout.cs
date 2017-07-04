using System;

namespace ATpayment
{
    class initiatecheckout
    {
        static public void Initiatecheckout()
        {
            //Create an instance of our awesome gateway class and pass your credentials
              string username = "MyAfricasTalkingUsername";
        string apiKey   = "MyAfricasTalkingAPIKey"; 
            // Specify the name of your Africa's Talking payment product
            string productName     = "ATproductName";
            // The phone number of the customer checking out
            string phoneNumber     = "++254711XXXYYY";
            // The 3-Letter ISO currency code for the checkout amount
            string currencyCode    = "KES";
            // The checkout amount
            int amount             = 500;
            // The provider Channel - Optional
            string providerChannel = "your providerChannel";

            // Create a new instance of our awesome gateway class
            AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey);

            // NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
            /*************************************************************************************
                                    ****SANDBOX****            
            **************************************************************************************/
           // AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey, "sandbox");
            // Any gateway errors will be captured by our custom Exception class below,
             
            try
            {
                // Initiate the checkout. If successful, you will get back a json response

                dynamic checkoutResponse = gateway.initiateMobilePaymentCheckout(productName, phoneNumber,currencyCode,amount,providerChannel);

                Console.WriteLine(checkoutResponse);

            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Encountered an error: " + e.Message);
            }
            Console.Read();
        }
    }
}
