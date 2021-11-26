using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using Chubb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubb.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISchoolContext context;


        public ProductRepository(ISchoolContext context)
        {
            this.context = context;
        }

        public Product Add(Product product)
        {
            var highestId = context.Products.Any() ? context.Products.Max(x => x.Id) : 1;
            product.Id = highestId + 1;
            context.Products.Add(product);
            return product;
        }

        public IEnumerable<Product> GetAll(Func<Product, bool> predicate = null)
        {
            if (predicate != null)
                return context.Products.Where(predicate).ToList();

            return context.Products.ToList();
        }

        public Product GetBy(Func<Product, bool> predicate = null)
        {
            if (predicate != null)
                return context.Products.FirstOrDefault(predicate);

            return null;
        }

        public bool Delete(int id)
        {

            var product = GetBy(x => x.Id == id);
            return context.Products.Remove(product);
        }

        public bool Update(Product product)
        {
            //Queda esta funcionalidad esta de más porque como esta en memoria automaticamente hace la actulización
            //Pero se mantiene debido a que esta de momento en moeria pero si se cambia el repositorio para que sea por BD sólo cambia la funcionalidad de las clases de repositorio
            //Por eso regresamos en automatico true
            return true;

        }
    }
}
