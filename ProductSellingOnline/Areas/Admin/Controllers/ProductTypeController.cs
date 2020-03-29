using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Data;

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


        
    }
}