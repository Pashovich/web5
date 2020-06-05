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
    public class LabsController : Controller
    {
        private readonly ApplicationDbContext context;

        public LabsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Labs
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Labs.ToListAsync());
        }


        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return this.NotFound();
            }

            return this.View(lab);
        }


        public IActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var lab = new Lab
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phones = new Collection<LabPhone>()
                };
                if (model.Phones != null)
                {
                    var phoneId = 1;
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        lab.Phones.Add(new LabPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                this.context.Add(lab);
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

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return this.NotFound();
            }

            var model = new LabEditModel
            {
                Name = lab.Name,
                Address = lab.Address,
                Phones = String.Join(", ", lab.Phones.OrderBy(x => x.PhoneId).Select(x => x.Number))
            };

            return this.View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, LabEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                lab.Name = model.Name;
                lab.Address = model.Address;
                var phoneId = lab.Phones.Any() ? lab.Phones.Max(x => x.PhoneId) + 1 : 1;
                lab.Phones.Clear();
                if (model.Phones != null)
                {
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        lab.Phones.Add(new LabPhone
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

            var lab = await this.context.Labs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return this.NotFound();
            }

            return this.View(lab);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var lab = await this.context.Labs.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Labs.Remove(lab);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
