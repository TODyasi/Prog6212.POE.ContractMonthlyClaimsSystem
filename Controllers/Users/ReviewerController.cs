using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers.Users
{
    public class ReviewerController : Controller
    {
        public IActionResult ReviewerView()
        {
            return View();
        }
    }
}
