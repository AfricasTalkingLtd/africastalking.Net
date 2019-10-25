using System;

namespace AfricasTalkingCS
{
    public class AfricasTalkingGatewayException : Exception
    {
        public AfricasTalkingGatewayException(string message) : base(message)
        {
        }

        public AfricasTalkingGatewayException(Exception exception) : base(exception.Message, exception)
        {
        }
    }
}