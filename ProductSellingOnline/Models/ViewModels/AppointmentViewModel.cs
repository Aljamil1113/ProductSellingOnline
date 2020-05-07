using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public List<Appointments> Appointments { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
