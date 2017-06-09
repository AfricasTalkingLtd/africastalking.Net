
    // Sending Messages using sender id/short code
    using System;
    class Smsshortcode
    {
        static public void _Smsshortcode (){
            
            string username = "MyAfricasTalkingUsername";
            string apiKey   = "MyAfricasTalkingAPIKey"; 
            
            string recipients = "+254711XXXYYY,+254733YYYZZZ";
            
            string message = "I'm a lumberjack and its ok, I sleep all night and I work all day";
            
            // Specify your AfricasTalking shortCode or sender id
            string from = "shortCode or senderId";
            
            AfricasTalkingGateway gateway = new AfricasTalkingGateway (username, apiKey);
            
            try {
                
                dynamic results = gateway.sendMessage (recipients, message, from);
                
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