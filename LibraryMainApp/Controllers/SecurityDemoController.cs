using Microsoft.AspNetCore.Mvc;

namespace LibraryMainApp.Controllers
{
    public class SecurityDemoController : Controller
    {
        public IActionResult LoginAsMember()
        {
            HttpContext.Session.SetString("Role", "Member");
            TempData["Success"] = "Demo login set as Member.";
            return RedirectToAction(nameof(AdminOnly));
        }

        public IActionResult LoginAsAdmin()
        {
            HttpContext.Session.SetString("Role", "Admin");
            TempData["Success"] = "Demo login set as Admin.";
            return RedirectToAction(nameof(AdminOnly));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Demo session cleared.";
            return RedirectToAction(nameof(AdminOnly));
        }

        public IActionResult AdminOnly()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role ?? "Not logged in";

            if (role != "Admin")
            {
                ViewBag.AccessMessage = "Access denied: this page is restricted to Admin users only.";
                return View();
            }

            ViewBag.AccessMessage = "Access granted: Admin can manage restricted library settings.";
            return View();
        }
    }
}
