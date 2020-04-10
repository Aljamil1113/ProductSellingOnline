using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Models.ViewModels
{
    public class ProductsViewModel
    {
        public Products Products { get; set; }
        public IEnumerable<ProductType> ProductTypes { get; set; }
        public IEnumerable<SpecialTag> SpecialTags { get; set; }
    }
}
