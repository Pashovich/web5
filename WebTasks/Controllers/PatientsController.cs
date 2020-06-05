﻿using System;
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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View(new PatientViewModel());
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Name = model.Name,
                    Phone = model.Phone
                };

                this._context.Patients.Add(patient);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);

        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return this.NotFound();
            }

            var model = new PatientViewModel
            {
                Name = patient.Name,
                Phone = patient.Phone
            };

            return this.View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PatientViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                patient.Name = model.Name;
                patient.Phone = model.Phone;

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

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
