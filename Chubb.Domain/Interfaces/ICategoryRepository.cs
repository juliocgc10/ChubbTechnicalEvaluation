using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Chubb.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll(Func<Category, bool> predicate = null);
    }
}
