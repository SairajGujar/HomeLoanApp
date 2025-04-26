using HomeLoan.Data;
using HomeLoan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeLoan.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult LoanApplications()
        {
            var applications = _db.LoanApplications.Include(l => l.User).ToList();
            return View(applications);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var loan = _db.LoanApplications.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            loan.Status = status;
            _db.SaveChanges();

            TempData["Success"] = $"Status updated to '{status}'";
            return RedirectToAction("LoanApplications");
        }

        public IActionResult ViewApplication(int id)
        {
            var loan = _db.LoanApplications
                .Include(l => l.User)
                .FirstOrDefault(l => l.Id == id);

            if (loan == null) return NotFound();

            return View(loan);
        }

        public IActionResult Dashboard()
        {
            ViewBag.PendingCount = _db.LoanApplications.Count(l => l.Status == "Pending");
            ViewBag.ApprovedCount = _db.LoanApplications.Count(l => l.Status == "Approved");
            ViewBag.RejectedCount = _db.LoanApplications.Count(l => l.Status == "Rejected");

            return View();
        }


    }
}
