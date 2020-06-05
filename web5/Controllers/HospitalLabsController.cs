using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web5.Data;
using web5.Models;
using web5.Models.ViewModels;

namespace web5.Controllers
{
    public class HospitalLabsController : Controller
    {
        private readonly ApplicationDbContext context;

        public HospitalLabsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: HospitalLabs
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            var items = await this.context.HospitalLabs
                .Include(h => h.Hospital)
                .Include(h => h.Lab)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();
            this.ViewBag.Hospital = hospital;
            return this.View(items);
        }

        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            return this.View(new HospitalLabCreateModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalLabCreateModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var hospitalLab = new HospitalLab
                {
                    HospitalId = hospital.Id,
                    LabId = model.LabId
                };

                this.context.Add(hospitalLab);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name", model.LabId);
            return this.View(model);
        }


        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? labId)
        {
            if (hospitalId == null || labId == null)
            {
                return this.NotFound();
            }

            var hospitalLab = await this.context.HospitalLabs
                .Include(h => h.Hospital)
                .Include(h => h.Lab)
                .SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            if (hospitalLab == null)
            {
                return this.NotFound();
            }

            return this.View(hospitalLab);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 labId)
        {
            var hospitalLab = await this.context.HospitalLabs.SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            this.context.HospitalLabs.Remove(hospitalLab);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = hospitalId });
        }
    }
}
