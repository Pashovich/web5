using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebTasks.Data;
using WebTasks.Models;
using WebTasks.Models.ViewModels;

namespace WebTasks.Controllers
{
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalDoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalDoctors
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            var items = await this._context.HospitalDoctors
                .Include(h => h.Hospital)
                .Include(h => h.Doctor)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();
            this.ViewBag.Hospital = hospital;
            return this.View(items);
        }


        // GET: HospitalDoctors/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["DoctorId"] = new SelectList(this._context.Doctors, "Id", "Name");
            return this.View(new HospitalDoctorViewModel());
        }

        // POST: HospitalDoctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalDoctorViewModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var hospitalDoctor = new HospitalDoctor
                {
                    HospitalId = hospital.Id,
                    DoctorId = model.DoctorId
                };

                this._context.Add(hospitalDoctor);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name", model.DoctorId);
            return this.View(model);
        }

       

        // GET: HospitalDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .SingleOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Delete/5
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
