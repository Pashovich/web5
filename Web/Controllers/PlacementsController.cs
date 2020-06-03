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

namespace Backend5.Controllers
{
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
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

            var placements = await _context.Placements.Include(w => w.Ward).Where(x => x.WardId == WardId).ToListAsync();

            return View(placements);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.Include(p => p.Ward).SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        public async Task<IActionResult> Create(Int32? WardId)
        {
            if (WardId == null)
            {
                return NotFound();
            };
            var ward = await _context.Wards.SingleOrDefaultAsync(m => m.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            };

            ViewBag.Ward = ward;
            return View(new PlacementForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? WardId ,PlacementForm model)
        {

            if (WardId == null)
            {
                return NotFound();
            };
            var ward = await _context.Wards.SingleOrDefaultAsync(m => m.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            };

            if (ModelState.IsValid)
            {

                var placement = new Placement
                {
                    Bed = model.Bed,
                    WardId = ward.Id
                };

                _context.Add(placement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = ward.Id });
            }

            ViewBag.Ward = ward;
            return View(model);
        }

        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            var model = new PlacementForm
            {
                Bed = placement.Bed
            };

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementForm model)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = placement.WardId });

            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.Include(p => p.Ward).SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = placement.WardId });
        }

        private bool PlacementExists(int id)
        {
            return _context.Placements.Any(e => e.Id == id);
        }
    }
}
