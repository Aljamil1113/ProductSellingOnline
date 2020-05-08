using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Models.ViewModels
{
    public class AdminUsersRolesViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string RoleName { get; set; }
        public List<string> Roles { get; set; }
    }
}
