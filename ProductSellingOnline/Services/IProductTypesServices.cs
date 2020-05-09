using ProductSellingOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Services
{
    public interface IProductTypesServices
    {
        ICollection<ProductType> GetProductTypes();
        ProductType GetProductType(int id);
        bool IsProductTypeExist(int id);
    }
}
