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
        // Implementation of DbContext from program.cs file
        private readonly ApplicationDbContext _claimsDb;

        // Constructor to initialize the ClaimsController with the ApplicationDbContext
        public ClaimsController(ApplicationDbContext claimsDb)
        {
            _claimsDb = claimsDb; // Assigning the passed ApplicationDbContext to the _claimsDb variable
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
            return View(); // Returning the view for creating a new claim
        }


        // The CreateClaim method with the [HttpPost] attribute processes the submitted claim data.
        // It creates a new claim object in the database and saves the changes.

        [HttpPost]
        public IActionResult CreateClaim(ClaimsBaseModel obj)
        {
            _claimsDb.Claims.Add(obj); // Adding the new claim object to the Claims DbSet
            _claimsDb.SaveChanges(); // Saving the changes to the database
            return RedirectToAction("Index", "Home"); // Redirecting to the Index view after creation
        }

        // The ChangeClaimStatus method updates the status of a claim based on the specified action (approve or reject).
        // It retrieves the claim by its ID, changes its status accordingly, and saves the changes to the database.
        // Finally, it redirects the user to the PendingClaims view.
        [HttpPost]
        public IActionResult ChangeClaimStatus(int claimId, string action)
        {
            var claim = _claimsDb.Claims.FirstOrDefault(c => c.ClaimId == claimId); // Retrieving the claim by its ID
            if (claim != null) // Checking if the claim exists
            {
                // Update the claim status based on the action
                if (action == "approve") // If the action is to approve
                {
                    claim.ClaimStatus = ClaimStatus.Approved; // Change to the approved status
                }
                else if (action == "reject") // If the action is to reject
                {
                    claim.ClaimStatus = ClaimStatus.Rejected; // Change to the rejected status
                }

                _claimsDb.SaveChanges(); // Saving the changes to the database
            }

            return RedirectToAction("PendingClaims"); // Redirecting back to the PendingClaims view
        }


        // The PendingClaims method retrieves all claims that have a status of Pending from the database.
        // It passes the list of pending claims to the view for display.
        public IActionResult PendingClaims()
        {
            var pendingClaims = _claimsDb.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Pending) // Filtering claims with status Pending
                .ToList(); // Converting the filtered claims to a list
            return View(pendingClaims); // Returning the view with the list of pending claims
        }

        // The RejectedClaims method retrieves all claims that have a status of Rejected from the database.
        // It passes the list of rejected claims to the view for display.
        public IActionResult RejectedClaims()
        {
            var rejectedClaims = _claimsDb.Claims
               .Where(c => c.ClaimStatus == ClaimStatus.Rejected) // Filtering claims with status Rejected
               .ToList(); // Converting the filtered claims to a list

            return View(rejectedClaims); // Returning the view with the list of Rejected claims
        }

        // The ApprovedClaims method retrieves all claims that have a status of Approved from the database.
        // It passes the list of pending claims to the view for display.
        public IActionResult ApprovedClaims()
        {
            var approvedClaims = _claimsDb.Claims
               .Where(c => c.ClaimStatus == ClaimStatus.Approved) // Filtering claims with status Approved
               .ToList(); // Converting the filtered claims to a list

            return View(approvedClaims); // Returning the view with the list of approved claims
        }
    }
}
