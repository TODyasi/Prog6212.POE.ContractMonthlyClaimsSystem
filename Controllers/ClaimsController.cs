using CMCS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers
{
    public class ClaimsController : Controller
    {
        //implementation of DbContext from program.cs file
        private readonly ApplicationDbContext _claimsDb;

        public ClaimsController(ApplicationDbContext claimsDb)
        {
            _claimsDb = claimsDb;
        }
        public IActionResult Index()
        {
            //Get claims and convert them to a list
            var claims = _claimsDb.Claims.ToList() ;
            return View(claims);
        }
    }
}
