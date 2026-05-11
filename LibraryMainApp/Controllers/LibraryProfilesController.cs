using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMainApp.Data;
using LibraryMainApp.Models;

namespace LibraryMainApp.Controllers
{
    public class LibraryProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LibraryProfiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LibraryProfile.Include(l => l.Admin);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LibraryProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryProfile = await _context.LibraryProfile
                .Include(l => l.Admin)
                .FirstOrDefaultAsync(m => m.LibraryID == id);
            if (libraryProfile == null)
            {
                return NotFound();
            }

            return View(libraryProfile);
        }

        // GET: LibraryProfiles/Create
        public IActionResult Create()
        {
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email");
            return View();
        }

        // POST: LibraryProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryID,StaffID,Name,Location,OperatingHours,Email,PhoneNumber,LoanDurationDays,MaxBorrowableBooks")] LibraryProfile libraryProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", libraryProfile.StaffID);
            return View(libraryProfile);
        }

        // GET: LibraryProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryProfile = await _context.LibraryProfile.FindAsync(id);
            if (libraryProfile == null)
            {
                return NotFound();
            }
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", libraryProfile.StaffID);
            return View(libraryProfile);
        }

        // POST: LibraryProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibraryID,StaffID,Name,Location,OperatingHours,Email,PhoneNumber,LoanDurationDays,MaxBorrowableBooks")] LibraryProfile libraryProfile)
        {
            if (id != libraryProfile.LibraryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryProfileExists(libraryProfile.LibraryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", libraryProfile.StaffID);
            return View(libraryProfile);
        }

        // GET: LibraryProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryProfile = await _context.LibraryProfile
                .Include(l => l.Admin)
                .FirstOrDefaultAsync(m => m.LibraryID == id);
            if (libraryProfile == null)
            {
                return NotFound();
            }

            return View(libraryProfile);
        }

        // POST: LibraryProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libraryProfile = await _context.LibraryProfile.FindAsync(id);
            if (libraryProfile != null)
            {
                _context.LibraryProfile.Remove(libraryProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryProfileExists(int id)
        {
            return _context.LibraryProfile.Any(e => e.LibraryID == id);
        }
    }
}
