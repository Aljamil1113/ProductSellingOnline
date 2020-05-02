using System.Collections.Generic;

namespace ProductSellingOnline.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Products> Products { get; set; }
        public Appointments Appointments { get; set; }
    }
}