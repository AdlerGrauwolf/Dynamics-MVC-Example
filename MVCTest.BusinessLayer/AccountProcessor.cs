using System;
using System.Linq;
using System.Collections.Generic;

using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.BusinessInterfaces;
using MVCTest.BusinessTypes.ExtensionMethods;

namespace MVCTest.BusinessLayer
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IAccountRepository _accountRepository;

        public AccountProcessor(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Guid CreateAccount(Account account)
        {
            if (account == null)
                throw new Exception("La cuenta contiene información nula");

            Guid newAccountId = Guid.Empty;

            account.Validate();

            newAccountId = _accountRepository.CreateAccount(account);

            return newAccountId;
        }

        public bool UpdateAccount(Account account)
        {
            if (account == null)
                throw new Exception("La cuenta contiene información nula");

            bool response = false;

            account.Validate();

            response = _accountRepository.UpdateAccount(account);

            return response;
        }

        public bool DeleteAccount(Guid accountId)
        {
            bool response = false;

            if (accountId != Guid.Empty && accountId != null)
                response = _accountRepository.DeleteAccount(accountId);

            return response;
        }

        public Account GetAccount(Guid accountId)
        {
            Account objAccount = new Account();

            if (accountId != null && accountId != Guid.Empty)
                objAccount = _accountRepository.GetAccount(accountId);

            return objAccount;
        }

        public List<Account> GetAccounts(string accountName = "")
        {
            List<Account> accounts = _accountRepository.GetAccounts(accountName.RemoveAccents());
            if (accounts != null && accounts.Any())
                return accounts;

            return new List<Account>();
        }
    }
}
