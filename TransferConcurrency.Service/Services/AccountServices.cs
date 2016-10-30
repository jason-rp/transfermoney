namespace TransferConcurrency.Service.Services
{
    using System.Collections.Generic;
    using Data;
    using System.Linq;
    using System.Data.Entity.Migrations;

    public class AccountServices
    {
        private static Dictionary<int, string> dictionnaries = new Dictionary<int, string>();

        public List<Account> GetAllAccounts() {
            using (var context = new TransferMoneyEntities())
            {
                return context.Accounts.ToList();
            }
        }
        public bool TransferMoney(int fromId, int toId, decimal balance)
        {
            var result = false;
            if (CheckAccountExist(fromId, toId, balance)) {
                AddDataToDictionary(fromId);
                AddDataToDictionary(toId);
                var order = new List<int> { fromId, toId }.OrderBy(n => n).ToList();
                lock (dictionnaries[order[0]])
                {
                    lock (dictionnaries[order[1]])
                    {
                        using (var context = new TransferMoneyEntities())
                        {
                            var fromAccount = context.Accounts.Find(fromId);
                            if (fromAccount.Balance >= balance) {
                                var toAccount = context.Accounts.Find(toId);
                                fromAccount.Withdraw(balance);
                                toAccount.Deposit(balance);
                                context.Accounts.AddOrUpdate(fromAccount);
                                context.Accounts.AddOrUpdate(toAccount);
                                context.SaveChanges();
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool CheckAccountExist(int fromId, int toId, decimal balance)
        {
            using (var context = new TransferMoneyEntities())
            {
                if (context.Accounts.Find(fromId) != null && context.Accounts.Find(toId) != null) return true;
                else return false;
            }
        }

        private void AddDataToDictionary(int accountId)
        {
            if (!dictionnaries.ContainsKey(accountId))
            {
                lock (dictionnaries)
                {
                    if (!dictionnaries.ContainsKey(accountId)) {
                        using (var context = new TransferMoneyEntities())
                        {
                            var account = context.Accounts.Find(accountId);
                            if (account != null) dictionnaries.Add(account.Id, account.Name);
                        }
                    }
                }
            }
        }
    }
}