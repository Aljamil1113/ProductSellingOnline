using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;

namespace ProductSellingOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext db;

        public SpecialTagController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {          
            return View(db.SpecialTags.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag tag)
        {
            if(ModelState.IsValid)
            {
                db.Add<SpecialTag>(tag);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var tag = await db.SpecialTags.FindAsync(id);

            if(tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            SpecialTag tag = await db.SpecialTags.FindAsync(id);

            if(tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialTag tag)
        {
            if(id != tag.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                db.SpecialTags.Update(tag);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var tag = await db.SpecialTags.FindAsync(id);

            if(tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            SpecialTag tag = await db.SpecialTags.FindAsync(id);
            db.SpecialTags.Remove(tag);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "SpecialTag");
        }
    }
}