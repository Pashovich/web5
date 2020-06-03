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
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalDoctorsController(ApplicationDbContext context)
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

            var items = await _context.HospitalDoctors.Include(h => h.Hospital).Include(h => h.Doctor).Where(x => x.HospitalId == hospital.Id).ToListAsync();
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            return View(new HospitalDoctorForm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalDoctorForm model)
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
                var hospitalDoctor = new HospitalDoctor
                {
                    HospitalId = hospital.Id,
                    DoctorId = model.DoctorId
                };

                _context.Add(hospitalDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            ViewBag.Hospital = hospital;
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", model.DoctorId);
            return View(model);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors.Include(h => h.Doctor).Include(h => h.Hospital).SingleOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalDoctor = await _context.HospitalDoctors.SingleOrDefaultAsync(m => m.HospitalId == id);
            _context.HospitalDoctors.Remove(hospitalDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalDoctorExists(int id)
        {
            return _context.HospitalDoctors.Any(e => e.HospitalId == id);
        }
    }
}
