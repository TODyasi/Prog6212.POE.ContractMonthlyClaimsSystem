using Microsoft.AspNetCore.Mvc;
using CMCS.Infrastructure.Data;
using CMCS.Domain.Entities.ClaimsModels;
using CMCS.Domain.Entities.ClaimsModels.Enums;
using System.Linq;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ClaimsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var claims = _dbContext.Claims.ToList();
            return View(claims);
        }

        public IActionResult CreateClaim()
        {
            return View(new ClaimsBaseModel());
        }

        [HttpPost]
        public IActionResult CreateClaim(ClaimsBaseModel obj)
        {
            if (obj.Title == obj.Description)
            {
                ModelState.AddModelError("title", "The description cannot exactly match the Title.");
            }

            _dbContext.Claims.Add(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Claim has been created successfully.";
            return RedirectToAction("LecturerView","Lecturer");
        }

        public IActionResult Update(int claimId)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [HttpPost]
        public IActionResult Update(ClaimsBaseModel obj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Claims.Update(obj);
                _dbContext.SaveChanges();
                TempData["success"] = "Claim has been updated successfully.";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Delete(int claimId)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int claimId)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                _dbContext.Claims.Remove(claim);
                _dbContext.SaveChanges();
                TempData["success"] = "Claim has been deleted successfully.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeClaimStatus(int claimId, string action)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.ClaimStatus = action == "approve" ? ClaimStatus.Approved : ClaimStatus.Rejected;
                _dbContext.SaveChanges();
                TempData["success"] = "Claim status has been updated successfully.";
            }

            return RedirectToAction("PendingClaims");
        }

        public IActionResult PendingClaims()
        {
            var pendingClaims = _dbContext.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Pending)
                .ToList();
            return View(pendingClaims);
        }

        public IActionResult RejectedClaims()
        {
            var rejectedClaims = _dbContext.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Rejected)
                .ToList();
            return View(rejectedClaims);
        }

        public IActionResult ApprovedClaims()
        {
            var approvedClaims = _dbContext.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Approved)
                .ToList();
            return View(approvedClaims);
        }
       

    }
}
