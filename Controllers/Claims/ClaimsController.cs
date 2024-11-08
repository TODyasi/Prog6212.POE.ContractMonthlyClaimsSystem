using CMCS.Domain.Entities.ClaimsModels;
using CMCS.Domain.Entities.ClaimsModels.Enums;
using CMCS.Domain.Entities.UserModels;
using CMCS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers
{
    // The ClaimsController class is responsible for managing claims in the application.
    // It handles operations related to viewing, creating, and updating the status of claims,
    // interacting with the database through the ApplicationDbContext.
    public class ClaimsController : Controller
    {
       
        private readonly ApplicationDbContext _claimsDb;
        public ClaimsController(ApplicationDbContext claimsDb)
        {
            _claimsDb = claimsDb; 
        }

        // The Index method retrieves all claims from the database and converts them into a list.
        // It then passes this list to the view for display.
        // This view will be returned when the administrator wants to view all the claims
        public IActionResult Index()
        {
            var claims = _claimsDb.Claims.ToList(); 
            return View(claims); 
        }

        // The CreateClaim method displays a form for creating a new claim.
        // It doesn't require any parameters and simply returns the view for claim creation.
        // This view will be returned when the user wants to create a new claim
        public IActionResult CreateClaim()
        {
            return View(); 
        }


        // The CreateClaim method with the [HttpPost] attribute processes the submitted claim data.
        // It creates a new claim object in the database and saves the changes.

        [HttpPost]
        public IActionResult CreateClaim(ClaimsBaseModel obj)
        {
            if(obj.Title == obj.Description)
            {
                ModelState.AddModelError("title", "The description cannot exactly match the Name.");
            }
            if(ModelState.IsValid)
            {
                _claimsDb.Claims.Add(obj);
                _claimsDb.SaveChanges();
                TempData["success"] = "Claim has been created successfully.";
                return RedirectToAction("LecturerView", "Lecturer");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Update(ClaimsBaseModel obj)
        {
            if (ModelState.IsValid && obj.ClaimId>0)
            {
                _claimsDb.Claims.Update(obj);
                _claimsDb.SaveChanges();
                TempData["success"] = "Claim has been updated successfully.";
                return RedirectToAction("LecturerView", "Lecturer");
            }
            return View(obj);
        }

        public IActionResult Update(int claimId)
        {
            ClaimsBaseModel obj = _claimsDb.Claims.FirstOrDefault(u => u.ClaimId == claimId);

            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return PartialView(obj);
        }

        
        public IActionResult Delete(int claimId)
        {
            ClaimsBaseModel obj = _claimsDb.Claims.FirstOrDefault(u => u.ClaimId == claimId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(ClaimsBaseModel obj)
        {
            ClaimsBaseModel? objFromDb = _claimsDb.Claims.FirstOrDefault(u => u.ClaimId == obj.ClaimId);
            if (objFromDb is not null)
            {
                _claimsDb.Claims.Remove(objFromDb);
                _claimsDb.SaveChanges();
                TempData["success"] = "Claim has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The claim could not be deleted";
            return View("Index");
        }


        // The ChangeClaimStatus method updates the status of a claim based on the specified action (approve or reject).
        // It retrieves the claim by its ID, changes its status accordingly, and saves the changes to the database.
        // Finally, it redirects the user to the PendingClaims view.
        [HttpPost]
        public IActionResult ChangeClaimStatus(int claimId, string action)
        {
            var claim = _claimsDb.Claims.FirstOrDefault(c => c.ClaimId == claimId); // Retrieving the claim by its ID
            if (claim != null)
            {
                // Update the claim status based on the action
                if (action == "approve")
                {
                    claim.ClaimStatus = ClaimStatus.Approved; 
                }
                else if (action == "reject") 
                {
                    claim.ClaimStatus = ClaimStatus.Rejected; 
                }

                _claimsDb.SaveChanges(); 
            }

            return RedirectToAction("PendingClaims"); 
        }


        // The PendingClaims method retrieves all claims that have a status of Pending from the database.
        // It passes the list of pending claims to the view for display.
        public IActionResult PendingClaims()
        {
            var pendingClaims = _claimsDb.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Pending) 
                .ToList(); 
            return View(pendingClaims);
        }

        // The RejectedClaims method retrieves all claims that have a status of Rejected from the database.
        // It passes the list of rejected claims to the view for display.
        public IActionResult RejectedClaims()
        {
            var rejectedClaims = _claimsDb.Claims
               .Where(c => c.ClaimStatus == ClaimStatus.Rejected) 
               .ToList();

            return View(rejectedClaims); 
        }

        // The ApprovedClaims method retrieves all claims that have a status of Approved from the database.
        // It passes the list of pending claims to the view for display.
        public IActionResult ApprovedClaims()
        {
            var approvedClaims = _claimsDb.Claims
               .Where(c => c.ClaimStatus == ClaimStatus.Approved) 
               .ToList(); 

            return View(approvedClaims); 
        }
        public IActionResult ComingSoon()
        {
            return View();
        }
    }
}
