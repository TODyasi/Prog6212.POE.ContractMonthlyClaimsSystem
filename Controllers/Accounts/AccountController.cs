using CMCS.Infrastructure.Data;
using CMCS.Domain.Entities.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers.Accounts
{
    public class AccountController : Controller
    {
        //access the database for users
        private readonly ApplicationDbContext _usersDb;

        public AccountController(ApplicationDbContext usersDb)
        {
            _usersDb = usersDb;
        }
        //registration page
        public IActionResult Register()
        {
            return View();
        }

        //login page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            //Add user to the database
          _usersDb.Users.Add(user);

            //Save changes to the database
            await _usersDb.SaveChangesAsync();

            //redirect to login page
            return RedirectToAction("Login");

        }
    }
}
