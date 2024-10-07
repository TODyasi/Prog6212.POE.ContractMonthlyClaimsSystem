using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
