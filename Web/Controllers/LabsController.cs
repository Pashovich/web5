using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class LabsController : Controller
    {
        private readonly ApplicationDbContext context;

        public LabsController(ApplicationDbContext context)
        {
            context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await context.Labs.ToListAsync());
        }

        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await context.Labs.Include(x => x.Phones).SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabCreateForm model)
        {
            if (ModelState.IsValid)
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

                context.Add(lab);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await context.Labs.Include(x => x.Phones).SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return NotFound();
            }

            var model = new LabEditForm
            {
                Name = lab.Name,
                Address = lab.Address,
                Phones = String.Join(", ", lab.Phones.OrderBy(x => x.PhoneId).Select(x => x.Number))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, LabEditForm model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await context.Labs.Include(x => x.Phones).SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lab = await context.Labs.SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var lab = await context.Labs.SingleOrDefaultAsync(m => m.Id == id);
            context.Labs.Remove(lab);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
