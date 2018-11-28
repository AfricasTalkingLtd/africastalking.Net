using System;

namespace VoiceMediaUpload
{
    using AfricasTalkingCS;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const string username    = "UserName";
            const string apikey      = "APIKEY";
            const string fileLocation = "http(s)://<url>.mp3/wav";
            const string phoneNumber = "callerID";

            var gateway = new AfricasTalkingGateway(username, apikey);

            try
            {
                var results = gateway.UploadMediaFile(fileLocation, phoneNumber);
                Console.WriteLine(results);
                
            }
            catch (AfricasTalkingGatewayException exception)
            {
                Console.WriteLine("Something went horribly wrong: " + exception.Message + ".\nCaused by :" + exception.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
