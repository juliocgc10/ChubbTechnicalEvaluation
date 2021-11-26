using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Chubb.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product GetBy(Func<Product, bool> predicate = null);
        IEnumerable<Product> GetAll(Func<Product, bool> predicate = null);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
