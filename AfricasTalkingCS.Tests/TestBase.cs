namespace AfricasTalkingCS.Tests
{
    public abstract class TestBase
    {
        private const string username = "sandbox";
        private const string environment = "sandbox";
        private const string apikey = "e952920d25a20cc9a8144ae200363d722f3459273815201914d8d4603e59d047";
        
        protected readonly AfricasTalkingGateway _atGWInstance;

        public TestBase()
        {
            _atGWInstance = new AfricasTalkingGateway(username, apikey,environment);
        }

        protected const string shortCode0 = "44000";

        protected const string phoneNumber0 = "+254720000000";
        protected const string phoneNumber1 = "+254720000001";
        protected const string phoneNumber2 = "+254720000002";
        protected const string phoneNumber3 = "+254720000003";

        protected const string amount100 = "KES 100";
    }
}