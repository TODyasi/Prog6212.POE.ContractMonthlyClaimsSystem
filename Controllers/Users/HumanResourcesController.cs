using CMCS.Application.Services;
using CMCS.Domain.Entities.ClaimsModels.Enums;
using CMCS.Domain.Entities.UserModels;
using CMCS.Domain.Entities.UserModels.Enums;
using CMCS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers
{
    public class HumanResourcesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HumanResourcesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var approvedClaims = _dbContext.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Approved)
                .ToList();
            return View(approvedClaims);
        }

        public IActionResult ManageLecturers()
        {
            var lecturers = _dbContext.Users
                .Where(u => u.Role == UserRole.Lecturer)
                .ToList();
            return View(lecturers);
        }

        public IActionResult CreateLecturer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLecturer(UserModel lecturer)
        {
            _dbContext.Users.Add(lecturer);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Lecturer created successfully!";
            return View(lecturer);
        }

        public IActionResult EditLecturer(int id)
        {
            var lecturer = _dbContext.Users
                .FirstOrDefault(u => u.UserId == id && u.Role == UserRole.Lecturer);

            if (lecturer == null)
            {
                TempData["ErrorMessage"] = "Lecturer not found.";
                return RedirectToAction("ManageLecturers");
            }

            return View(lecturer);
        }

        [HttpPost]
        public IActionResult EditLecturer(UserModel lecturer)
        {
            var existingLecturer = _dbContext.Users
                .FirstOrDefault(u => u.UserId == lecturer.UserId);

            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Lecturer updated successfully!";
            return RedirectToAction("ManageLecturers");
        }

        public IActionResult DeleteLecturer(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteLecturerConfirmed(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Lecturer deleted successfully!";
            return RedirectToAction("ManageLecturers");
        }

        public IActionResult HRReports()
        {
            var approvedClaims = _dbContext.Claims
                .Where(c => c.ClaimStatus == ClaimStatus.Approved)
                .ToList();
            return View(approvedClaims);
        }

        [HttpPost]
        public IActionResult GenerateInvoice(int claimId)
        {
            var claim = _dbContext.Claims
                .FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                TempData["Message"] = "Claim not found.";
                return RedirectToAction("HRReports");
            }

            var invoicePdf = PdfService.GenerateInvoicePdf(claim);
            return invoicePdf;
        }

        [HttpPost]
        public IActionResult GenerateSummaryReport(int claimId)
        {
            var claim = _dbContext.Claims
                .FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                TempData["Message"] = "Claim not found.";
                return RedirectToAction("HRReports");
            }

            var reportPdf = PdfService.GenerateSummaryReportPdf(claim);
            return reportPdf;
        }

    }
}
