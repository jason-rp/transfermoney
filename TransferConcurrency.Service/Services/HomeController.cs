namespace TransferConcurrency.Service.Services
{
    using Controllers;
    using Data;
    using System.Web.Mvc;
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var accounts = new AccountController().GetAllAccounts();
            return View(accounts);
        }
    }
}