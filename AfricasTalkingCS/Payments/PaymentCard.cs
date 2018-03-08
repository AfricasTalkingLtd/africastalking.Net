namespace AfricasTalkingSDK.Payments
{
    struct PaymentCard
    {
        string number;
        int cvvNumber;
        int expiryMonth;
        int expiryYear;
        string countryCode;
        string authToken;

        public override string ToString()
        {
            return "{" +
                $"number:{number},cvvNumber:{cvvNumber}," +
                $"expiryMonth:{expiryMonth},expiryYear:{expiryYear}," +
                $"countryCode:{countryCode},authToken:{authToken}" + "}";
        }
    }
}
