using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;
using ProductSellingOnline.Utility;

namespace ProductSellingOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminUser)]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminUsersController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View(db.ApplicationUser.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var userFromDb = await db.ApplicationUser.FindAsync(id);

            if(userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            if(id != applicationUser.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                ApplicationUser userFromDb = db.ApplicationUser.Where(a => a.Id == id).FirstOrDefault();
                userFromDb.FullName = applicationUser.FullName;
                userFromDb.FirstName = applicationUser.FirstName;
                userFromDb.LastName = applicationUser.LastName;
                userFromDb.MiddleName = applicationUser.MiddleName;
                userFromDb.CompleteAddress = applicationUser.CompleteAddress;
                userFromDb.PhoneNumber = applicationUser.PhoneNumber;

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFromDb = await db.ApplicationUser.FindAsync(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            if (id == null)
            {
                return NotFound();
            }   
            ApplicationUser userFromDb = db.ApplicationUser.Where(a => a.Id == id).FirstOrDefault();
            //userFromDb.LockoutEnd = DateTime.Now.AddDays(1);
            db.Remove(userFromDb);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));          
        }
    }
}