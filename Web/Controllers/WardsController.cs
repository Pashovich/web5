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
    public class WardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return View(await _context.Wards.ToListAsync());
            }

            var hospital = await _context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return NotFound();
            }

            ViewBag.Hospital = hospital;
            var wards = await _context.Wards.Include(w => w.Hospital).Where(x => x.HospitalId == hospitalId).ToListAsync();

            return View(wards);
        }

        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.Include(w => w.Hospital).SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            return View(ward);
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
            return View(new WardCreateForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, WardCreateForm model)
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
                var ward = new Ward
                {
                    HospitalId = hospital.Id,
                    Name = model.Name
                };

                _context.Add(ward);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            ViewBag.Hospital = hospital;
            return View(model);
        }

        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            var model = new WardEditForm
            {
                Name = ward.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardEditForm model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ward.Name = model.Name;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { hospitalId = ward.HospitalId });
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.Include(w => w.Hospital).Include(w => w.WardStaffs).SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            return View(ward);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var ward = await _context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            _context.Wards.Remove(ward);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { hospitalId = ward.HospitalId });
        }
    }
}
