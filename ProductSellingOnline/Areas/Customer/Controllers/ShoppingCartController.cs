using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
using ProductSellingOnline.Extensions;
using ProductSellingOnline.Models;
using ProductSellingOnline.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public ShoppingCartController(ApplicationDbContext _db)
        {
            db = _db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Products>()
            };
        }

        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if(lstShoppingCart == null)
            {
                return PartialView("~/Views/Shared/_NoCartItems.cshtml");
            }

            if(lstShoppingCart.Count > 0)
            {
                foreach(int cartItem in lstShoppingCart)
                {
                    Products prod = await db.Products.Include(p => p.ProductType).Include(s => s.SpecialTag).Where(p => p.Id == cartItem).FirstOrDefaultAsync();
                    ShoppingCartVM.Products.Add(prod);
                }
            }
            return View(ShoppingCartVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            ShoppingCartVM.Appointments.AppointmentDate = ShoppingCartVM.Appointments.AppointmentDate
                                                            .AddHours(ShoppingCartVM.Appointments.AppointmentTime.Hour)
                                                            .AddMinutes(ShoppingCartVM.Appointments.AppointmentTime.Minute);

            Appointments appointments = ShoppingCartVM.Appointments;
            await db.Appointments.AddAsync(appointments);
            await db.SaveChangesAsync();

            int appointmentId = appointments.Id;

            foreach(int cart in lstCartItems)
            {
                ProductSelectedForAppointment productAppointments = new ProductSelectedForAppointment()
                {
                   AppointmentId = appointmentId,
                   ProductId = cart
                };
               
                await db.ProductSelectedForAppointment.AddAsync(productAppointments);
            }
           
            await db.SaveChangesAsync();

            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction("AppointmentDetails", "ShoppingCart", new { id = appointmentId });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if(lstCartItems.Contains(id))
            {
                lstCartItems.Remove(id);
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("Index");
        }

        public IActionResult AppointmentDetails(int id)
        {
            ShoppingCartVM.Appointments = db.Appointments.Where(a => a.Id == id).FirstOrDefault();
            List<ProductSelectedForAppointment> prod = db.ProductSelectedForAppointment.Where(a => a.AppointmentId == id).ToList();

            foreach(var cart in prod)
            {
                ShoppingCartVM.Products.Add(db.Products.Include(s => s.SpecialTag).Include(p => p.ProductType).Where(a => a.Id == cart.ProductId).FirstOrDefault());
            }
            return View(ShoppingCartVM);
        }
    }
}