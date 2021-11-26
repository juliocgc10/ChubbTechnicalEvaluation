using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using Chubb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubb.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IChubbContext context;


        public CategoryRepository(IChubbContext context)
        {
            this.context = context;
        }

      

        public IEnumerable<Category> GetAll(Func<Category, bool> predicate = null)
        {

            if (predicate != null)
                return context.Categories.Where(predicate);

            return context.Categories;

        }

       
        
    }
}
