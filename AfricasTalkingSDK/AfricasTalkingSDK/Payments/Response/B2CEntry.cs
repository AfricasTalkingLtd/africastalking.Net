namespace AfricasTalkingSDK.Payments.Response
{
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