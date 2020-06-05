using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class HospitalsController : Controller
    {
        private readonly ApplicationDbContext context;

        public HospitalsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Hospitals.ToListAsync());
        }


        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            return this.View(hospital);
        }

        public IActionResult Create()
        {
            return this.View(new HospitalCreateModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var hospital = new Hospital
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phones = new Collection<HospitalPhone>()
                };
                if (model.Phones != null)
                {
                    var phoneId = 1;
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        hospital.Phones.Add(new HospitalPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                this.context.Hospitals.Add(hospital);
                await this.context.SaveChangesAsync();
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

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            var model = new HospitalEditModel
            {
                Name = hospital.Name,
                Address = hospital.Address,
                Phones = String.Join(", ", hospital.Phones.OrderBy(x => x.PhoneId).Select(x => x.Number))
            };

            return this.View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, HospitalEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                hospital.Name = model.Name;
                hospital.Address = model.Address;
                var phoneId = hospital.Phones.Any() ? hospital.Phones.Max(x => x.PhoneId) + 1 : 1;
                hospital.Phones.Clear();
                if (model.Phones != null)
                {
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        hospital.Phones.Add(new HospitalPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }


        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            return this.View(hospital);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var hospital = await this.context.Hospitals.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Hospitals.Remove(hospital);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
