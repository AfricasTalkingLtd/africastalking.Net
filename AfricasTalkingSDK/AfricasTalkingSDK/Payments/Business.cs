using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments
{
    struct Business
    {
        string amount;
        string provider;
        string transferType;
        string destinationChannel;
        string destinationAccount;
        Dictionary<string, string> metadata;

        public override string ToString()
        {
            return "{" +
                   $"amount={amount},provider:{provider}" +
                   $"transferType:{transferType},destinationChannel:{destinationChannel}" +
                   $"destinationAccount:{destinationAccount},metadata:{metadata}" +
                   "}";
        }
    }
}
