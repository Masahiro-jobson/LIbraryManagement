using LibraryMainApp.Data;
using LibraryMainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMainApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check Staff first
            var staff = await _context.Staffs
                .FirstOrDefaultAsync(s => s.Email == email
                                       && s.PasswordHash == password);

            if (staff != null)
            {
                HttpContext.Session.SetInt32("UserID", staff.StaffID);
                HttpContext.Session.SetString("UserName", staff.FirstName);
                HttpContext.Session.SetString("Role", "Staff");
                return RedirectToAction("Dashboard", "Librarian");
            }

            // Check Member
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == email
                                       && m.PasswordHash == password);

            if (member != null)
            {
                HttpContext.Session.SetInt32("UserID", member.MemberID);
                HttpContext.Session.SetString("UserName", member.FirstName);
                HttpContext.Session.SetString("Role", "Member");
                return RedirectToAction("Dashboard", "Member");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email,
                                                   string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var exists = await _context.Members
                .AnyAsync(m => m.Email == email);

            if (exists)
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }

            var nameParts = fullName.Split(' ', 2);

            var member = new Member
            {
                FirstName = nameParts[0],
                LastName = nameParts.Length > 1 ? nameParts[1] : "",
                Email = email,
                PasswordHash = password,
                Address = "",
                PhoneNumber = "",
                RegistrationDate = DateTime.Now
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
