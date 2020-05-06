using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;
using ProductSellingOnline.Models.ViewModels;
using ProductSellingOnline.Utility;

namespace ProductSellingOnline.Areas.Admin.Controllers
{
    //[Authorize(Roles = SD.AdminUser + "," + SD.SuperAdminUser)]
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext db;
        

        public AppointmentsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index(string searchName=null, string searchEmail=null, string searchPhone=null, string searchDate=null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentView = new AppointmentViewModel()
            {
                Appointments = new List<Appointments>()
            };

            appointmentView.Appointments = db.Appointments.Include(s => s.applicationUser).ToList();

            if(User.IsInRole(SD.AdminUser))
            {
                appointmentView.Appointments = appointmentView.Appointments.Where(a => a.SalesPersonId == claim.Value).ToList();
            }

            if(searchName!=null)
            {
                appointmentView.Appointments = db.Appointments.Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }

            if (searchEmail != null)
            {
                appointmentView.Appointments = db.Appointments.Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();
            }

            if (searchPhone != null)
            {
                appointmentView.Appointments = db.Appointments.Where(a => a.CustomerPhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToList();
            }

            if (searchDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchDate);
                    appointmentView.Appointments = db.Appointments.Where(a => a.AppointmentDate.ToShortDateString().Contains(appDate.ToShortDateString())).ToList();
                }
                catch(Exception ex)
                {

                }
                
            }

            return View(appointmentView);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var ProductsList = (IEnumerable<Products>)(from p in db.Products
                            join pa in db.ProductSelectedForAppointment
                            on p.Id equals pa.ProductId
                            where pa.AppointmentId == id
                            select p).Include("ProductType");

            AppointmentDetialsViewModel appointmentDetailsView = new AppointmentDetialsViewModel()
            {
                Appointment = db.Appointments.Include(a => a.applicationUser).Where(a => a.Id == id).FirstOrDefault(),
                SalesPerson = db.ApplicationUser.ToList(),
                Products = ProductsList.ToList()
            };

            return View(appointmentDetailsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentDetialsViewModel objAppointment)
        {
            if(ModelState.IsValid)
            {
                objAppointment.Appointment.AppointmentDate = objAppointment.Appointment.AppointmentDate
                    .AddHours(objAppointment.Appointment.AppointmentTime.Hour)
                    .AddMinutes(objAppointment.Appointment.AppointmentTime.Minute);

                var appointmentDb = db.Appointments.Where(a => a.Id == objAppointment.Appointment.Id).FirstOrDefault();

                appointmentDb.CustomerName = objAppointment.Appointment.CustomerName;
                appointmentDb.CustomerEmail = objAppointment.Appointment.CustomerEmail;
                appointmentDb.CustomerPhoneNumber = objAppointment.Appointment.CustomerPhoneNumber;
                appointmentDb.AppointmentDate = objAppointment.Appointment.AppointmentDate;
                appointmentDb.IsConfirmed = objAppointment.Appointment.IsConfirmed;
                appointmentDb.SalesPersonId = objAppointment.Appointment.SalesPersonId;

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(objAppointment);
        }
    }
}