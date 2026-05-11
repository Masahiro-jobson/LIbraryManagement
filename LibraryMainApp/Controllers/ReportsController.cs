using LibraryMainApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMainApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.OverdueBooks = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.DueDate < DateTime.Now)
                .OrderBy(l => l.DueDate)
                .ToListAsync();

            ViewBag.MostBorrowedBooks = await _context.Loans
                .GroupBy(l => new { l.ISBN, l.Book.Title })
                .Select(g => new PopularBookReport
                {
                    ISBN = g.Key.ISBN,
                    Title = g.Key.Title,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(5)
                .ToListAsync();

            ViewBag.ActiveMembers = await _context.Loans
                .GroupBy(l => new { l.MemberID, l.Member.FirstName, l.Member.LastName })
                .Select(g => new ActiveMemberReport
                {
                    MemberID = g.Key.MemberID,
                    Name = g.Key.FirstName + " " + g.Key.LastName,
                    LoanCount = g.Count()
                })
                .OrderByDescending(x => x.LoanCount)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }

    public class PopularBookReport
    {
        public int ISBN { get; set; }
        public string Title { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class ActiveMemberReport
    {
        public int MemberID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LoanCount { get; set; }
    }
}
