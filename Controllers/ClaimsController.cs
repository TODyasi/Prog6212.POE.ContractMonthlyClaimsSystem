using CMCS.Domain.Entities.ClaimsModels;
using CMCS.Domain.Entities.ClaimsModels.Enums;
using CMCS.Domain.Entities.UserModels;
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

        public IActionResult CreateClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateClaim(ClaimsBaseModel obj)
        {
            // create a new claim object
            _claimsDb.Claims.Add(obj);
            
            _claimsDb.SaveChanges();

            //redirect to the index view after claim has been created
            return RedirectToAction("Index", "Claims");
        }
        [HttpPost]
        public IActionResult ChangeClaimStatus(int claimId, string action)
        {
            // Retrieve the claim by its ID
            var claim = _claimsDb.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                // Update the claim status based on the action
                if (action == "approve")
                {
                    claim.ClaimStatus = ClaimStatus.Approved; // Change to the appropriate approved status
                }
                else if (action == "reject")
                {
                    claim.ClaimStatus = ClaimStatus.Rejected; // Change to the appropriate rejected status
                }

                // Save the changes to the database
                _claimsDb.SaveChanges();
            }

            // Redirect back to the PendingClaims view
            return RedirectToAction("PendingClaims");
        }

        public IActionResult PendingClaims()
        {
            // Retrieve all claims with a status of Pending
            var pendingClaims = _claimsDb.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Pending)
                .ToList();

            // Pass the list of pending claims to the view
            return View(pendingClaims);
        }
    }
}
