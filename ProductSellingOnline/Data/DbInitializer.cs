using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Models;
using ProductSellingOnline.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Data
{
    public class DbInitializer : IDbInitializer
    {
        public ApplicationDbContext db { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public RoleManager<IdentityRole> roleManager { get; set; }

        public DbInitializer(ApplicationDbContext _db, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _userRoles)
        {
            db = _db;
            userManager = _userManager;
            roleManager = _userRoles;
        }
        public async void Initializer()
        {
            if(db.Database.GetPendingMigrations().Count() > 0 )
            {
                db.Database.Migrate();
            }

            if (db.Roles.Any(r => r.Name == SD.SuperAdminUser)) return;

            roleManager.CreateAsync(new IdentityRole(SD.AdminUser)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(SD.SuperAdminUser)).GetAwaiter().GetResult();

            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                MiddleName = "Admin",
                EmailConfirmed = true                
            }, "Admin123*").GetAwaiter().GetResult();

            IdentityUser user = await db.Users.Where(u => u.Email == "admin@gmail.com").FirstOrDefaultAsync();

            await userManager.AddToRoleAsync(user, SD.SuperAdminUser);
        }
    }
}

