using System;
class Sendsms
{
    static public void Sendsmg()
    {

        // Specify your login credentials
         string username = "MyAfricasTalkingUsername";
        string apiKey   = "MyAfricasTalkingAPIKey"; 
        // Specify the numbers that you want to send to in a comma-separated list
        // Please ensure you include the country code (+254 for Kenya in this case)
        string recipients = "+254711XXXYYY";
        // And of course we want our recipients to know what we really do
        string message = "I'm a lumberjack and its ok, I sleep all night and I work all day";

        // Create a new instance of our awesome gateway class
        AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey);
        // Any gateway errors will be captured by our custom Exception class below,
        // so wrap the call in a try-catch block   
        try
        {
            // Thats it, hit send and we'll take care of the rest

            dynamic results = gateway.sendMessage(recipients, message);

            foreach (dynamic result in results)
            {
                Console.Write((string)result["number"] + ",");
                Console.Write((string)result["status"] + ","); // status is either "Success" or "error message"
                Console.Write((string)result["messageId"] + ",");
                Console.WriteLine((string)result["cost"]);
            }
        }
        catch (AfricasTalkingGatewayException e)
        {
            Console.WriteLine("Encountered an error: " + e.Message);
        }
        Console.Read();
    }
}