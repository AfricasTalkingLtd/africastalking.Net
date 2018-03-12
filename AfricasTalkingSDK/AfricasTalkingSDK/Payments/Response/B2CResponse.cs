using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments.Response
{
    class B2CResponse
    {
        public int numQueued;
        public string totalValue;
        public string totalTransactionFee;
        public List<B2CEntry> entries;
    }
}
