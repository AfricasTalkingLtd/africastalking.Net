using System;
using System.Collections.Generic;

public class TestMobilePaymentB2C
{
    public static void MobilePaymentB2C()
    {
        //Specify your credentials
         string username = "MyAfricasTalkingUsername";
        string apiKey   = "MyAfricasTalkingAPIKey"; 

        // NOTE: If connecting to the sandbox, please use your sandbox login credentials

        //Create an instance of our awesome gateway class and pass your credentials
        // AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey,"sandbox");

        // NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
        /// <summary>
        ///***********************************************************************************
        ///                    ****SANDBOX****
       AfricasTalkingGateway gateway    = new AfricasTalkingGateway(username, apiKey, "production");
        /// *************************************************************************************
        /// </summary>

        // Specify the name of your Africa's Talking payment product
        string productName = "My Online Store";

        // The 3-Letter ISO currency code for the checkout amount
        string currencyCode = "KES";
      

        // Provide the details of a mobile money recipient
        MobilePaymentB2CRecipient recipient1 = new MobilePaymentB2CRecipient("+254700YYYXXX", "KES", 10M);
        recipient1.AddMetadata("name", "Clerk");
        recipient1.AddMetadata("reason", "May Salary");

        // You can provide up to 10 recipients at a time
       MobilePaymentB2CRecipient recipient2 = new MobilePaymentB2CRecipient("+254741YYYXXX", "KES", 10M);
       recipient2.AddMetadata("name", "Accountant");
       recipient2.AddMetadata("reason", "May Salary");

        // Put the recipients into an array
        IList<MobilePaymentB2CRecipient> recipients = new List<MobilePaymentB2CRecipient>();
        recipients.Add(recipient1);
         recipients.Add(recipient2);

        try
        {
            var responses = gateway.MobilePaymentB2CRequest(productName, recipients);
            Console.WriteLine(responses);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Received error response: " + ex.Message);
        }
        Console.ReadLine();
    }
}