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
    public class HospitalLabsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalLabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return NotFound();
            }

            var items = await _context.HospitalLabs.Include(h => h.Hospital).Include(h => h.Lab).Where(x => x.HospitalId == hospital.Id).ToListAsync();
            ViewBag.Hospital = hospital;
            return View(items);
        }

        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return NotFound();
            }

            ViewBag.Hospital = hospital;
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
            return View(new HospitalLabCreateForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalLabCreateForm model)
        {
            if (hospitalId == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hospitalLab = new HospitalLab
                {
                    HospitalId = hospital.Id,
                    LabId = model.LabId
                };

                _context.Add(hospitalLab);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            ViewBag.Hospital = hospital;
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", model.LabId);
            return View(model);
        }

        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? labId)
        {
            if (hospitalId == null || labId == null)
            {
                return NotFound();
            }

            var hospitalLab = await _context.HospitalLabs.Include(h => h.Hospital).Include(h => h.Lab).SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            if (hospitalLab == null)
            {
                return NotFound();
            }

            return View(hospitalLab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 labId)
        {
            var hospitalLab = await _context.HospitalLabs.SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            _context.HospitalLabs.Remove(hospitalLab);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { hospitalId = hospitalId });
        }
    }
}
