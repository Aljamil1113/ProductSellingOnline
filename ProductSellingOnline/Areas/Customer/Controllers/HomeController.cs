using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
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
        public IActionResult Details(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }

            var product = db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).SingleOrDefault(x => x.Id == Id);

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
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
