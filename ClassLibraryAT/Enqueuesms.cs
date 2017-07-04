    //Message queueing 
    using System;
using System.Collections;
    class MainClass
    {
        static public void Enqueuesms (){
            
            string username = "MyAfricasTalkingUsername";
            string apiKey   = "MyAfricasTalkingAPIKey"; 
            
            string recipients = "+254700XXXYYY,+254733YYYZZZ";
            
            string message = "I'm a lumberjack and its ok, I sleep all night and I work all day";
            
            string from = null; //$from = "shortCode or senderId";
            
            int bulkSMSMode = 1; // This should always be 1 for bulk messages
            
            // enqueue flag is used to queue messages incase you are sending a high volume.
            // The default value is 0.
            Hashtable options = new Hashtable();
            options["enqueue"] = 1;
            
            AfricasTalkingGateway gateway = new AfricasTalkingGateway (username, apiKey);
            
            try {
                
                dynamic results = gateway.sendMessage (recipients, message, from, bulkSMSMode, options);
                
                foreach( dynamic result  in results){
                    Console.Write((string)result["number"] + ",");
                    Console.Write((string)result["status"] + ",");
                    Console.Write((string)result["messageId"] + ",");
                    Console.WriteLine((string)result["cost"]);
                }
            } catch (AfricasTalkingGatewayException e) {
                Console.WriteLine ("Encountered an error: " + e.Message);        
            }
            Console.Read();
        }
    }