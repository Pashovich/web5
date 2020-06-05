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
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        { 
            return View(new DoctorViewModel());
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Name = model.Name,
                    Speciality = model.Speciality
                };
                
                this._context.Doctors.Add(doctor);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);

        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            var model = new DoctorViewModel
            {
                Name = doctor.Name,
                Speciality = doctor.Speciality
            };

            return this.View(model);
        }


        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, DoctorViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                doctor.Name = model.Name;
                doctor.Speciality = model.Speciality;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }


        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.SingleOrDefaultAsync(m => m.Id == id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
