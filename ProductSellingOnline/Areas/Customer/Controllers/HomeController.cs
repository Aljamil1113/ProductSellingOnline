using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
using ProductSellingOnline.Extensions;
using ProductSellingOnline.Models;
using ProductSellingOnline.Models.ViewModels;

namespace ProductSellingOnline.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private int PageSize = 5;

        public HomeController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index(int productPage = 1, string searchProductName=null, string searchPrice=null)
        {
            HomeProductsViewModel prodView = new HomeProductsViewModel
            {
                Products = new List<Products>()
            };

            StringBuilder param = new StringBuilder();

            param.Append("/Products?productPage=:");

            param.Append("&searchProductName=");
            if(searchProductName != null)
            {
                param.Append(searchProductName);
            }

            param.Append("&searchPrice=");
            if(searchPrice != null)
            {
                param.Append(searchPrice);
            }

            prodView.Products = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).ToListAsync();

            if (searchProductName != null)
            {
                prodView.Products = await db.Products.Where(p => p.Name.ToLower().Contains(searchProductName.ToLower()) ||
                p.ProductType.Name.ToLower().Contains(searchProductName.ToLower()) || 
                p.SpecialTag.Name.ToLower().Contains(searchProductName.ToLower())).ToListAsync();
                
            }

            if (searchPrice != null)
            {
                try
                {
                    int maxPrice = Convert.ToInt32(searchPrice);
                    prodView.Products = await db.Products.Where(p => p.Price <= maxPrice).ToListAsync();
                }
                catch (Exception ex)
                {
                }
            }

            var count = prodView.Products.Count;

            prodView.Products = prodView.Products.OrderBy(p => p.Name)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();

            prodView.PageInfo = new PageInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };

            //var productList = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).ToListAsync();
            return View(prodView);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).Where(m => m.Id == id).FirstOrDefaultAsync();
            return View(product);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lssShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            var product = await db.Products.FindAsync(id);
            if(lssShoppingCart == null)
            {
                lssShoppingCart = new List<int>();
            }
            lssShoppingCart.Add(id);
            
            HttpContext.Session.Set("ssShoppingCart", lssShoppingCart);
          
            return RedirectToAction("Index", "Home", new{area = "Customer"});
        }

        public async Task<IActionResult> Remove(int id)
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
