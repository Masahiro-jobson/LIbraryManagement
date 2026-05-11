using LibraryMainApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMainApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Role check - only Members can access
            if (HttpContext.Session.GetString("Role") != "Member")
                return RedirectToAction("Login", "Account");

            var memberID = HttpContext.Session.GetInt32("UserID");

            // Fetch current loans for this member
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.MemberID == memberID)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

            return View(loans);
        }
    }
}
