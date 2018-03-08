using AfricasTalkingSDK.Account;
using Newtonsoft.Json;

namespace AfricasTalkingSDK
{
    class AccountService : Service
    {
        private dynamic _instance;

        AccountService(string username, string apiKey) : base(username, apiKey, "accounts") { }

        protected override dynamic GetInstance(string username, string apiKey)
        {
            if (_instance == null)
            {
                _instance = new AccountService(username, apiKey);
            }
            return _instance;
        }


        public AccountResponse FetchAccount()
        {
            var response = MakeRequest("user");
            AccountResponse accountResponse = JsonConvert.DeserializeObject<AccountResponse>(response);
            return accountResponse;
        }
    }
}
