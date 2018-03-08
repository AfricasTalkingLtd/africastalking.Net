namespace AfricasTalkingSDK.Payments.Response
{
    public class CheckoutResponse
    {

        /**
         * Unique transaction ID
         */
        public string transactionId;
        /**
         * Transaction status e.g. CheckoutResponse.STATUS_PENDING
         */
        public string status;

        /**
         * Status description
         */
        public string description;


        /**
         * Optional checkout token
         */
        public string checkoutToken = null;

    }
}
