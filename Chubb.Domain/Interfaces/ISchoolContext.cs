using Chubb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Domain.Interfaces
{
    public interface ISchoolContext
    {
        List<Product> Products { get; set; }
        List<Category> Categories { get; set; }
    }
}
