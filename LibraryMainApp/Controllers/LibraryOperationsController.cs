using LibraryMainApp.Data;
using LibraryMainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMainApp.Controllers
{
    public class LibraryOperationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public LibraryOperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.OrderBy(b => b.Title).ToListAsync();
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int isbn)
        {
            // Redirect to login if not logged in
            if (HttpContext.Session.GetString("Role") != "Member")
            {
                TempData["Error"] = "Please login as a member to borrow books.";
                return RedirectToAction("Login", "Account");
            }

            var memberID = HttpContext.Session.GetInt32("UserID") ?? 1;
            var staffID = 1;

            var book = await _context.Books.FindAsync(isbn);
            if (book == null) return NotFound();

            if (book.AvailabilityStatus != "Available")
            {
                TempData["Error"] = "This book is not available. Please reserve it instead.";
                return RedirectToAction(nameof(Index));
            }

            var loan = new Loan
            {
                MemberID = memberID,
                StaffID = staffID,
                ISBN = isbn,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14)
            };

            book.AvailabilityStatus = "Borrowed";
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"'{book.Title}' borrowed successfully. Due date: {loan.DueDate:dd MMM yyyy}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int isbn)
        {
            // Redirect to login if not logged in
            if (HttpContext.Session.GetString("Role") != "Member")
            {
                TempData["Error"] = "Please login as a member to reserve books.";
                return RedirectToAction("Login", "Account");
            }

            var memberID = HttpContext.Session.GetInt32("UserID") ?? 1;
            var staffID = 1;

            var book = await _context.Books.FindAsync(isbn);
            if (book == null) return NotFound();

            if (book.AvailabilityStatus == "Available")
            {
                TempData["Error"] = "This book is available. You can borrow it instead of reserving it.";
                return RedirectToAction(nameof(Index));
            }

            var alreadyReserved = await _context.Reservations.AnyAsync(r =>
                r.ISBN == isbn && r.MemberID == memberID && r.Status == "Active");

            if (alreadyReserved)
            {
                TempData["Error"] = "You already have an active reservation for this book.";
                return RedirectToAction(nameof(Index));
            }

            var reservation = new Reservation
            {
                MemberID = memberID,
                StaffID = staffID,
                ISBN = isbn,
                ReservationDate = DateTime.Now,
                Status = "Active"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Reservation created for '{book.Title}'.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Loans()
        {
            var role = HttpContext.Session.GetString("Role");
            var userID = HttpContext.Session.GetInt32("UserID");

            IQueryable<Loan> query = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.ReturnDate == null);   // active loans only

            // Members only see their own loans
            if (role == "Member" && userID.HasValue)
                query = query.Where(l => l.MemberID == userID.Value);

            var loans = await query.OrderByDescending(l => l.LoanDate).ToListAsync();
            return View(loans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.LoanID == loanId);

            if (loan == null) return NotFound();

            loan.ReturnDate = DateTime.Now;
            loan.Book.AvailabilityStatus = "Available";

            var staffID = HttpContext.Session.GetInt32("UserID") ?? 1;

            if (DateTime.Now.Date > loan.DueDate.Date)
            {
                var overdueDays = (DateTime.Now.Date - loan.DueDate.Date).Days;
                var fine = new Fine
                {
                    LoanID = loan.LoanID,
                    StaffID = staffID,
                    FineAmount = overdueDays * 1.00m,
                    FineDate = DateTime.Now,
                    Status = "Unpaid"
                };
                _context.Fines.Add(fine);
                TempData["Error"] = $"Book returned late. Fine generated: ${fine.FineAmount}.";
            }
            else
            {
                TempData["Success"] = "Book returned successfully with no fine.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Loans));
        }
    }
}
