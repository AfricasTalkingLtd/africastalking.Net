
namespace BankTransfer
{
    using System;
    using System.Collections.Generic;

    using AfricasTalkingCS;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string username = "sandbox";
            const string apikey = "afd635a4f295dd936312836c0b944d55f2a836e8ff2b63987da5e717cd5ff745";
            const string productname = "coolproduct";
            const string env = "sandbox";
            var gateway = new AfricasTalkingGateway(username, apikey, env);
            var currency_code = "NGN";
            var recipient1_account_name = "Alyssa Hacker";
            var recipient1_account_number = "1234567890";
            var recipient1_bank_code = 234001;
            decimal recipient1_amount = 1500.50M;
            var recipient1_narration = "December Bonus";
            var recipient2_account_name = "Ben BitDiddle";
            var recipient2_account_number = "234567891";
            var recipient2_bank_code = 234004;
            decimal recipient2_amount = 1500.50M;
            var recipient2_narration = "November Bonus";
            var recepient1_account = new BankAccount(recipient1_account_number, recipient1_bank_code, recipient1_account_name);
            var recepient1 = new BankTransferRecipients(recipient1_amount, recepient1_account, currency_code, recipient1_narration);
            recepient1.AddMetadata("Reason", "Early Bonus");
            var recipient2_account = new BankAccount(recipient2_account_number, recipient2_bank_code, recipient2_account_name);
            var recipient2 = new BankTransferRecipients(recipient2_amount, recipient2_account, currency_code, recipient2_narration);
            recipient2.AddMetadata("Reason", "Big Wins");
            IList<BankTransferRecipients> recipients = new List<BankTransferRecipients>
                                                           {
                                                               recepient1,
                                                               recipient2
                                                           };
            try
            {
               var res = gateway.BankTransfer(productname, recipients);
                Console.WriteLine(res);
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("We had issues: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
