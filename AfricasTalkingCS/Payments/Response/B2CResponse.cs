using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments.Response
{
    class B2CResponse
    {
        public int numQueued;
        public string totalValue;
        public string totalTransactionFee;
        public List<B2CEntry> entries;


        public class B2CEntry
        {
            public string phoneNumber;
            public string status;
            public string provider;
            public string providerChannel;
            public string value;
            public string transactionId;
            public string transactionFee;
            public string errorMessage = null;
        }
    }
}
