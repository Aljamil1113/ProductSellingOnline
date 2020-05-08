using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;
using ProductSellingOnline.Models.ViewModels;
using ProductSellingOnline.Utility;

namespace ProductSellingOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminUser)]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext db;

        [BindProperty]
        public AdminUsersRolesViewModel AdminUsersRolesVM { get; set; }

        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public AdminUsersController(ApplicationDbContext _db, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            db = _db;
            roleManager = _roleManager;
            userManager = _userManager;
            AdminUsersRolesVM = new AdminUsersRolesViewModel()
            {
                ApplicationUser = new ApplicationUser(),
                RoleName = "",
                Roles = new List<string>()
            };
        }

        public IActionResult Index()
        {
            return View(db.ApplicationUser.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var roles = roleManager.Roles.ToList();

            if(id == null)
            {
                return NotFound();
            }

            AdminUsersRolesVM.ApplicationUser = await db.ApplicationUser.FindAsync(id);

            List<string> listRoles = new List<string>();
            foreach (var item in roles)
            {
                listRoles.Add(item.Name);
            }

            if(AdminUsersRolesVM.ApplicationUser == null)
            {
                return NotFound();
            }

            AdminUsersRolesVM.RoleName = userManager.GetRolesAsync(AdminUsersRolesVM.ApplicationUser).Result.Count != 0 ? userManager.GetRolesAsync(AdminUsersRolesVM.ApplicationUser).Result[0] : "";
            AdminUsersRolesVM.Roles = listRoles;
            return View(AdminUsersRolesVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {
            if(id != AdminUsersRolesVM.ApplicationUser.Id)
            {
                return NotFound();
            }

            var roles = roleManager.Roles.ToList();

            var user = userManager.FindByIdAsync(id).Result;

            if(ModelState.IsValid)
            {
                ApplicationUser userFromDb = db.ApplicationUser.Where(a => a.Id == AdminUsersRolesVM.ApplicationUser.Id).FirstOrDefault();
                userFromDb.FullName = AdminUsersRolesVM.ApplicationUser.FullName;
                userFromDb.FirstName = AdminUsersRolesVM.ApplicationUser.FirstName;
                userFromDb.LastName = AdminUsersRolesVM.ApplicationUser.LastName;
                userFromDb.MiddleName = AdminUsersRolesVM.ApplicationUser.MiddleName;
                userFromDb.CompleteAddress = AdminUsersRolesVM.ApplicationUser.CompleteAddress;
                userFromDb.PhoneNumber = AdminUsersRolesVM.ApplicationUser.PhoneNumber;

                List<string> listRoles = new List<string>();

                foreach (var item in roles)
                {
                    listRoles.Add(item.Name);
                }

                await userManager.RemoveFromRolesAsync(user, listRoles);
                await userManager.AddToRoleAsync(user, AdminUsersRolesVM.RoleName);

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(AdminUsersRolesVM);
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