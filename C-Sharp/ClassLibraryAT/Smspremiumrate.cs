    // Sending premium rated messages
    using System;
    using System.Collections;
    class Smspremiumrate
    {
        static public void _Smspremiumrate(){
            
            string username = "MyAfricasTalkingUsername";
            string apiKey   = "MyAfricasTalkingAPIKey"; 
            
            string recipients = "+254711XXXYYY,+254733YYYZZZ";
            
            string message = "Get your daily message and that's how we roll.";
            
            // Specify your premium shortCode and keyword
            string shortCode = "XXXXX";
            string keyword   = "premiumKeyword";
            
            // Set the bulkSMSMode flag to 0 so that the subscriber get charged
            int bulkSMSMode = 0;
            
            // Create an array which would hold the following parameters:
            // keyword: Your premium keyword,
            // retryDurationInHours: The numbers of hours our API should retry to send the message 
            // incase it doesn't go through. It is optional
            
            Hashtable options = new Hashtable();
            options["keyword"] = keyword;
            options["retryDurationInHours"] = "No. of hours to retry sending message";
            
            AfricasTalkingGateway gateway = new AfricasTalkingGateway (username, apiKey);
            
            try {
                
                dynamic results = gateway.sendMessage (recipients, message, shortCode, bulkSMSMode, options);
                
                foreach( dynamic result  in results){
                    Console.Write((string)result["number"] + ",");
                    Console.Write((string)result["status"] + ",");
                    Console.WriteLine((string)result["messageId"]);
                }
            } catch (AfricasTalkingGatewayException e) {
                Console.WriteLine ("Encountered an error: " + e.Message);        
            }
        }
    }