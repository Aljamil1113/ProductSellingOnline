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
    }
}