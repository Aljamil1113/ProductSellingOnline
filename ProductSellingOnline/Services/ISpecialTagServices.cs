using ProductSellingOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellingOnline.Services
{
    public interface ISpecialTagServices
    {
        ICollection<SpecialTag> GetSpecialTags();
        SpecialTag GetSpecialTag(int id);
        bool IsSpecialTagExist(int id);
    }
}
