using ProductSellingOnline.Data;
using ProductSellingOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Services
{
    public class SpecialTagServices : ISpecialTagServices
    {
        private readonly ApplicationDbContext db;

        public SpecialTagServices(ApplicationDbContext _db)
        {
            db = _db;
        }
        public SpecialTag GetSpecialTag(int id)
        {
            return db.SpecialTags.Where(s => s.Id == id).FirstOrDefault();
        }

        public ICollection<SpecialTag> GetSpecialTags()
        {
            return db.SpecialTags.ToList();
        }

        public bool IsSpecialTagExist(int id)
        {
            return db.SpecialTags.Any(s => s.Id == id);
        }
    }
}
