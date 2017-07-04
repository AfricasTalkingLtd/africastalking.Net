    //Sending onDemand premium messages
    using System;
using System.Collections;
    class Smspremiumondemand
    {
        static public void Smgpremiumondemand(){
            
            string username = "MyAfricasTalkingUsername";
            string apiKey   = "MyAfricasTalkingAPIKey"; 
            
            string recipients = "+254711XXXYYY,+254733YYYZZZ";
            
            string message = "Get your daily message and thats how we roll.";
            
            string shortCode = "XXXXX";
            string keyword   = "premiumKeyword"; // string keyword = null;
            
            int bulkSMSMode = 0;
            
            // Create a hashtable which would hold the parameters keyword, retryDurationInHours and linkId
            // linkId is received from the message sent by subscriber to your onDemand service
            string linkId = "messageLinkId";
            
            Hashtable options = new Hashtable();
            options["keyword"] = keyword;
            options["linkId"] = linkId;
            options["retryDurationInHours"] = "No. of hours to retry sending message";
            
            AfricasTalkingGateway gateway = new AfricasTalkingGateway (username, apiKey);
            
            try {
                
                dynamic results = gateway.sendMessage (recipients, message, shortCode, bulkSMSMode, options);
                
                foreach( dynamic result  in results){
                    Console.Write((string)result["number"] + ",");
                    Console.Write((string)result["status"] + ",");
                    Console.Write((string)result["messageId"] + ",");
                }
            } catch (AfricasTalkingGatewayException e) {
                Console.WriteLine ("Encountered an error: " + e.Message);        
            }
        }
    }