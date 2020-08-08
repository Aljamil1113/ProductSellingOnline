using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Models.ViewModels
{
    public class HomeProductsViewModel
    {
        public List<Products> Products { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
