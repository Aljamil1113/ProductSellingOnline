using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;
using ProductSellingOnline.Models.ViewModels;
using ProductSellingOnline.Utility;

namespace ProductSellingOnline.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminUser)]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db;
        private IHostingEnvironment hosting;

        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }

        public ProductsController(ApplicationDbContext _db, IHostingEnvironment _hosting)
        {
            db = _db;
            hosting = _hosting;
            ProductsVM = new ProductsViewModel()
            {
                ProductTypes = db.ProductTypes.ToList(),
                SpecialTags = db.SpecialTags.ToList(),
                Products = new Products()
            };
        }


        public async Task<IActionResult> Index()
        {
            var products = db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag);

            return View(await products.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(ProductsVM);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if(ModelState.IsValid)
            {
                var prod = await db.Products.AddAsync(ProductsVM.Products);
                await db.SaveChangesAsync();

                //======= SAVED IMAGE ============/
                string webRootPath = hosting.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productsFromDb = db.Products.Find(ProductsVM.Products.Id);

                if (files.Count != 0)
                {
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;
                }

                else
                {
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                    System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png");
                    productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png";
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Products");
            }

            return View(ProductsVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).SingleOrDefaultAsync(p => p.Id == id);

            if(ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if(id != ProductsVM.Products.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                string webRootPath = hosting.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = db.Products.Where(m => m.Id == ProductsVM.Products.Id).FirstOrDefault();

                if(files.Count > 0 && files[0] != null)
                {
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(ProductsVM.Products.Image);

                    if(System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.Id + extension_old));
                    }

                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension_new;
                }

                if(ProductsVM.Products.Image != null)
                {
                    productFromDb.Image = ProductsVM.Products.Image;
                }

                productFromDb.Name = ProductsVM.Products.Name;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.Available = ProductsVM.Products.Available;
                productFromDb.ProductTypeId = ProductsVM.Products.ProductTypeId;
                productFromDb.SpecialTagId = ProductsVM.Products.SpecialTagId;
                productFromDb.ShadeColor = ProductsVM.Products.ShadeColor;

                //db.Products.Update(productFromDb);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products =  db.Products.Include(pt => pt.ProductType).Include(s => s.SpecialTag).SingleOrDefault((p => p.Id == id));

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
              string webRootPath = hosting.WebRootPath;
              Products prod = await db.Products.FindAsync(id);

              if(prod == null)
              {
                return NotFound();
              }

              else
              {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(prod.Image);

                if(System.IO.File.Exists(Path.Combine(uploads, prod.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, prod.Id + extension));
                }

                db.Products.Remove(prod);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

             
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products =  db.Products.Include(pt => pt.ProductType).Include(s => s.SpecialTag).SingleOrDefault(p => p.Id == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }
    }
}
