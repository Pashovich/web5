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
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctors.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doc = await _context.Doctors.SingleOrDefaultAsync(item => item.Id == id);
            if (doc == null)
            {
                return NotFound();
            }

            return View(doc);
        }

        public IActionResult Create()
        { 
            return View(new DoctorForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorForm model)
        {
            if (this.ModelState.IsValid)
            {
                var doc = new Doctor
                {
                    Name = model.Name,
                    Speciality = model.Speciality
                };
                
                this._context.Doctors.Add(doc);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);

        }

        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doc = await this._context.Doctors.SingleOrDefaultAsync(item => item.Id == id);
            if (doc == null)
            {
                return this.NotFound();
            }

            var modelDoctor = new DoctorForm
            {
                Name = doc.Name,
                Speciality = doc.Speciality
            };

            return this.View(modelDoctor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, DoctorForm model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doc = await this._context.Doctors.SingleOrDefaultAsync(item => item.Id == id);
            if (doc == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                doc.Name = model.Name;
                doc.Speciality = model.Speciality;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doc = await _context.Doctors.SingleOrDefaultAsync(item => item.Id == id);
            if (doc == null)
            {
                return NotFound();
            }

            return View(doc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doc = await _context.Doctors.SingleOrDefaultAsync(item => item.Id == id);
            _context.Doctors.Remove(doc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(item => item.Id == id);
        }
    }
}
