using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductSellingOnline.Data;
using ProductSellingOnline.Models;

namespace ProductSellingOnline.Services
{
    public class ProductTypesServices : IProductTypesServices
    {
        private readonly ApplicationDbContext db;

        public ProductTypesServices(ApplicationDbContext _db)
        {
            db = _db;
        }

        public ProductType GetProductType(int id)
        {
            return db.ProductTypes.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<ProductType> GetProductTypes()
        {
            return db.ProductTypes.OrderBy(n => n.Name).ToList();
        }

        public bool IsProductTypeExist(int id)
        {
            return db.ProductTypes.Any(p => p.Id == id);
        }
    }
}
