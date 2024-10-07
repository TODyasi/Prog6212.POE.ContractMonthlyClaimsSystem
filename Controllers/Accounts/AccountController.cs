using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers.Accounts
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
