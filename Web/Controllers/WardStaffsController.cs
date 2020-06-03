using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.Forms;

namespace Web.Controllers
{
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Int32? WardId)
        {
            if (WardId == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            }

            ViewBag.Ward = ward;

            var wardstaff = await _context.WardStaff
                .Where(x => x.WardId == WardId)
                .ToListAsync();

            return View(wardstaff);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff.Include(w => w.Ward).SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        public async Task<IActionResult> Create(Int32? WardId)
        {
            if (WardId == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            }

            ViewBag.Ward = ward;
            return View(new WardStaffForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? WardId, WardStaffForm model)
        {
            if (WardId == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                var WS = new WardStaff
                {
                    Name = model.Name,
                    Position = model.Position,
                    WardId = ward.Id
                };

                _context.Add(WS);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = ward.Id });
            }
            ViewBag.Ward = ward;
            return View(model);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff.SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Position,WardId")] WardStaff wardStaff)
        {
            if (id != wardStaff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wardStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WardStaffExists(wardStaff.Id))
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
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var wardStaff = await _context.WardStaff.SingleOrDefaultAsync(m => m.Id == id);
            _context.WardStaff.Remove(wardStaff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = wardStaff.WardId });
        }

        private bool WardStaffExists(Int32 id)
        {
            return _context.WardStaff.Any(e => e.Id == id);
        }
    }
}
