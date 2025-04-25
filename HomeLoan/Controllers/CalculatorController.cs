using Microsoft.AspNetCore.Mvc;

namespace HomeLoan.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Eligibility()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Eligibility(decimal MonthlyIncome, decimal ExistingEMI)
        {
            // Very basic logic: 60x monthly income - existing EMI * 12
            decimal eligibleAmount = (MonthlyIncome * 60) - (ExistingEMI * 12);
            eligibleAmount = Math.Max(eligibleAmount, 0);

            ViewBag.EligibleAmount = Math.Round(eligibleAmount, 2);
            return View();
        }
        [HttpGet]
        public IActionResult EMI()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EMI(decimal LoanAmount, decimal InterestRate, int TenureYears)
        {
            var r = (InterestRate / 12) / 100;
            var n = TenureYears * 12;
            var emi = (LoanAmount * r * (decimal)Math.Pow(1 + (double)r, n)) /
                      ((decimal)Math.Pow(1 + (double)r, n) - 1);

            ViewBag.EMI = Math.Round(emi, 2);
            return View();
        }


    }
}
