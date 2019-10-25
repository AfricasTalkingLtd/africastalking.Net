// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankAccount.cs" company="Africa's Talking">
//   2019
// </copyright>
// <summary>
//   Defines the BankAccout type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace AfricasTalkingCS
{
    /// <summary>
    ///     The bank account.
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BankAccount" /> class.
        /// </summary>
        /// <param name="accountNumber">
        ///     The account number.
        /// </param>
        /// <param name="bankCode">
        ///     A 6-Digit Integer Code for the bank that we allocate.
        /// </param>
        /// <param name="dateOfBirth">
        ///     Date of birth of the account owner. Required for Zenith Nigeria.Format (YYYY-MM-DD).
        /// </param>
        /// <param name="accountName">
        ///     The name of the bank account
        /// </param>
        public BankAccount(string accountNumber, int bankCode, string dateOfBirth = null, string accountName = null)
        {
            AccountName = accountName;
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            BankCode = bankCode;
            DateOfBirth = dateOfBirth;
        }

        /// <summary>
        ///     Gets or sets the account name.
        ///     The name of the bank account.
        /// </summary>
        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        /// <summary>
        ///     Gets or sets the account number.
        ///     The account number.
        /// </summary>
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        /// <summary>
        ///     Gets or sets the bank code.
        ///     A 6-Digit Integer Code for the bank that we allocate.
        /// </summary>
        [JsonProperty("bankCode")]
        public int BankCode { get; set; }

        /// <summary>
        ///     Gets or sets the date of birth.
        ///     Date of birth of the account owner. Required for Zenith Nigeria.(YYYY-MM-DD).
        /// </summary>
        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}