using LibraryMainApp.Data;
using LibraryMainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibraryMainApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // New arrivals - 4 most recently added books
            var newArrivals = await _context.Books
                .OrderByDescending(b => b.ISBN)
                .Take(4)
                .ToListAsync();

            // Most borrowed books
            var mostBorrowed = await _context.Loans
                .GroupBy(l => l.ISBN)
                .Select(g => new { ISBN = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(4)
                .Join(_context.Books, g => g.ISBN, b => b.ISBN,
                      (g, b) => new { b.Title, b.Author, b.AvailabilityStatus, g.Count })
                .ToListAsync();

            ViewBag.NewArrivals = newArrivals;
            ViewBag.MostBorrowed = mostBorrowed;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
