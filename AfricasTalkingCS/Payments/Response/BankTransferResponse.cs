using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments.Response
{
    public class BankTransferResponse
    {
        public string errorMessage;
        public List<BankEntries> entries;

        public class BankEntries
        {
            public string accountNumber;
            public string status;
            public string transactionId;
            public string transactionFee;
            public string errorMessage;
        }
    }
}
