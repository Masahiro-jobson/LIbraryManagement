using LibraryMainApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMainApp.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrarianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Librarian/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Role check - only Staff can access
            if (HttpContext.Session.GetString("Role") != "Staff")
                return RedirectToAction("Login", "Account");

            // Fetch 5 most recent loans for activity table
            var recentLoans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .OrderByDescending(l => l.LoanDate)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentLoans = recentLoans;

            return View();
        }
    }
}
