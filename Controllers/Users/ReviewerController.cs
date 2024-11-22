
using CMCS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers.Users
{
    public class ReviewerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ReviewerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult ReviewerView()
        {
            var claims = _dbContext.Claims.ToList();
            return View(claims);
        }

        public IActionResult FilterClaims(float? minHoursWorked, float? maxHoursWorked, float? minHourlyRate, float? maxHourlyRate, string claimType)
        {
            //Get all the claims
            var claims = _dbContext.Claims.AsQueryable();

            //Apply filters
            if (minHoursWorked.HasValue)
            {
                claims = claims.Where(c => c.HoursWorked >= minHoursWorked);
            }
            if (maxHoursWorked.HasValue)
            {
                claims = claims.Where(c => c.HoursWorked <= maxHoursWorked);
            }
            if (minHourlyRate.HasValue)
            {
                claims = claims.Where(c => c.HourlyRate >= minHourlyRate);
            }
            if (maxHourlyRate.HasValue)
            {
                claims = claims.Where(c => c.HourlyRate <= maxHourlyRate);
            }
            if (!string.IsNullOrEmpty(claimType))
            {
                claims = claims.Where(c => c.ClaimType.Contains(claimType));
            }

            //Display the filtered list
            return View("ReviewerView", claims.ToList());
        }

    }
}
