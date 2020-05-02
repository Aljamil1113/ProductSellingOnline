using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductSellingOnline.Models;

namespace ProductSellingOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet <SpecialTag> SpecialTags { get; set; }
        public DbSet<Products> Products { get; set; }

        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<ProductSelectedForAppointment> ProductSelectedForAppointment { get; set; }
    }
}
