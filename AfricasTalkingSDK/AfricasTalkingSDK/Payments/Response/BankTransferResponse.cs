using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments.Response
{
    class BankTransferResponse
    {
        public string errorMessage;
        public List<BankEntries> entries;
    }
}
