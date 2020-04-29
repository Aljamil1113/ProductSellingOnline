using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
using ProductSellingOnline.Extensions;
using ProductSellingOnline.Models;

namespace ProductSellingOnline.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            var productList = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).ToListAsync();
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var product = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).Where(m => m.Id == Id).FirstOrDefaultAsync();
            return View(product);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int Id)
        {
            List<int> lssShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if(lssShoppingCart == null)
            {
                lssShoppingCart = new List<int>();
            }
            lssShoppingCart.Add(Id);
            HttpContext.Session.Set("ssShoppingCart", lssShoppingCart);
            return RedirectToAction("Index", "Home", new{area = "Customer"});
        }

        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart.Count > 0)
            {
                lstShoppingCart.Remove(id);
            }

            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
