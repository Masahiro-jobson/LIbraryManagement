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
    public class FinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fines.Include(f => f.Loan).Include(f => f.Staff);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Loan)
                .Include(f => f.Staff)
                .FirstOrDefaultAsync(m => m.FineID == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }

        // GET: Fines/Create
        public IActionResult Create()
        {
            ViewData["LoanID"] = new SelectList(_context.Loans, "LoanID", "LoanID");
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email");
            return View();
        }

        // POST: Fines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FineID,LoanID,StaffID,FineAmount,FineDate,Status")] Fine fine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoanID"] = new SelectList(_context.Loans, "LoanID", "LoanID", fine.LoanID);
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", fine.StaffID);
            return View(fine);
        }

        // GET: Fines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines.FindAsync(id);
            if (fine == null)
            {
                return NotFound();
            }
            ViewData["LoanID"] = new SelectList(_context.Loans, "LoanID", "LoanID", fine.LoanID);
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", fine.StaffID);
            return View(fine);
        }

        // POST: Fines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FineID,LoanID,StaffID,FineAmount,FineDate,Status")] Fine fine)
        {
            if (id != fine.FineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineExists(fine.FineID))
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
            ViewData["LoanID"] = new SelectList(_context.Loans, "LoanID", "LoanID", fine.LoanID);
            ViewData["StaffID"] = new SelectList(_context.Staffs, "StaffID", "Email", fine.StaffID);
            return View(fine);
        }

        // GET: Fines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Loan)
                .Include(f => f.Staff)
                .FirstOrDefaultAsync(m => m.FineID == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }

        // POST: Fines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fine = await _context.Fines.FindAsync(id);
            if (fine != null)
            {
                _context.Fines.Remove(fine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FineExists(int id)
        {
            return _context.Fines.Any(e => e.FineID == id);
        }
    }
}
