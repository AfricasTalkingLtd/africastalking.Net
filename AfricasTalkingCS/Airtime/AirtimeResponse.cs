using System.Collections.Generic;

namespace AfricasTalkingSDK
{
    class AirtimeResponse
    {
        public int numSent;
        public string totalAmount;
        public string totalDiscount;
        public string errorMessage;
        public List<AirtimeEntry> responses;

        public class AirtimeEntry
        {
            public string errorMessage;
            public string phoneNumber;
            public string amount;
            public string discount;
            public string status;
            public string requestId;
        }
    }
}
