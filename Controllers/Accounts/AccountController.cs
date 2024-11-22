using CMCS.Infrastructure.Data;
using CMCS.Domain.Entities.UserModels;
using Microsoft.AspNetCore.Mvc;
using CMCS.Domain.Entities.UserModels.Enums;

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
            // Check if a user with the same email already exists
            if (_usersDb.Users.Any(u => u.Email == user.Email))
            {
                ViewData["Message"] = "An account with this email already exists.";
                return View(user); 
            }
            //Add user to the database
            _usersDb.Users.Add(user);

            //Save changes to the database
            await _usersDb.SaveChangesAsync();

            //redirect to login page
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Validate user credentials
            var user = _usersDb.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ViewBag.Message = "Invalid email or password.";
                return View();
            }
            switch (user.Role)
            {
                case UserRole.Lecturer:
                    return RedirectToAction("LecturerView", "Lecturer");

                case UserRole.ProgramCoordinator:
                    return RedirectToAction("ReviewerView", "Reviewer");

                case UserRole.AcademicManager:
                    return RedirectToAction("ReviewerView", "Reviewer");
                case UserRole.HRManager:
                    return RedirectToAction("Index", "HumanResources");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }


    }
}
