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
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductTypeController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View(db.ProductTypes.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType prodTypes)
        {
            if(ModelState.IsValid)
            {
                db.ProductTypes.Add(prodTypes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "ProductType");
            }

            return View(prodTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();

            }
            ProductType Ptype = await db.ProductTypes.FindAsync(id);

            if(Ptype == null)
            {
                return NotFound();
            }
            return View(Ptype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if(id != productType.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                db.ProductTypes.Update(productType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "ProductType");
            }

            return View(productType);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var PType = await db.ProductTypes.FindAsync(id);

            if(PType == null)
            {
                return NotFound();
            }

            return View(PType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var productType = await db.ProductTypes.FindAsync(id);
            db.ProductTypes.Remove(productType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "ProductType");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var productType = await db.ProductTypes.FindAsync(id);

            if(productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }
    }
}