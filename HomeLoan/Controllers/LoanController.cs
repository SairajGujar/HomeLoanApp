using HomeLoan.Data;
using HomeLoan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HomeLoan.Controllers
{
    [Authorize(Roles = "User")]
    public class LoanController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoanController(UserManager<IdentityUser> userManager, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);


            return View();
        }
        [HttpPost]
        public IActionResult Create(LoanApplication loanApplication, IFormFile? AadharFile, IFormFile? PANFile, IFormFile? SalarySlip, IFormFile? NOCFILE)
        {
            
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                loanApplication.AadharFilePath = SaveFile(AadharFile);
                loanApplication.PANFilePath = SaveFile(PANFile);
                loanApplication.SalarySlipPath = SaveFile(SalarySlip);
                loanApplication.NOCFilePath = SaveFile(NOCFILE);
                loanApplication.UserId = userId;
                _db.LoanApplications.Add(loanApplication);
                _db.SaveChanges();
                TempData["Success"] = "Loan application submitted successfully!";
                return RedirectToAction("Index", "Home");

            }
            return View(loanApplication);

        }
        

        private string SaveFile(IFormFile file)
        {
            if (file == null) return null;
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadDir); // ensure folder exists   

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return "/uploads/" + fileName;
        }

        public async Task<IActionResult> Track()
        {
            var user = await _userManager.GetUserAsync(User);

            var applications = _db.LoanApplications
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            return View(applications);
        }


    }
}
