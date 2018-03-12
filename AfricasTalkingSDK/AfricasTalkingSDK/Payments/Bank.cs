using System.Collections.Generic;

namespace AfricasTalkingSDK.Payments
{
    class Bank
    {
        readonly string _bankAccount;
        readonly string _amount;
        readonly string _narration;
        readonly Dictionary<string, string> _metadata;

        public Bank(string bankAccount, string amount, string narration, Dictionary<string, string> metadata)
        {
            _bankAccount = bankAccount;
            _amount = amount;
            _narration = narration ?? "";
            _metadata = metadata;
        }

        public override string ToString()
        {
            return "{" + $"bankAccount:{_bankAccount}, amount:{_amount}," +
                   $"narration: {_narration},metadata:{_metadata}" + "}";
        }
    }
}
