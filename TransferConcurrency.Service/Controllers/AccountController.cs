namespace TransferConcurrency.Service.Controllers
{
    using System.Web.Http;
    using Services;
    using System.Collections.Generic;
    using Data;

    public class AccountController : ApiController
    {
        public bool Transfer(int fromId,int toId, decimal balance) {
            return new AccountServices().TransferMoney(fromId, toId, balance);
        }

        public List<Account> GetAllAccounts() {
            return new AccountServices().GetAllAccounts();
        }
    }
}
