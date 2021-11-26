using Chubb.Domain.Entities;
using Chubb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Repository
{
    public class SchoolContext : IChubbContext
    {
        private List<Product> _products;
        public List<Product> Products
        {
            get
            {
                if (_products == null)
                    SetProducts();

                return _products;
            }
            set { _products = value; }
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get
            {
                if (_categories == null)
                    SetCategories();

                return _categories;
            }
            set { _categories = value; }
        }

        public SchoolContext()
        {

            SetCategories();

            SetProducts();
        }

        private void SetProducts()
        {
            _products = new List<Product>() {
            new Product(){ Id = 1, Name = "Leche", Description = "Alpura 1L",UnitPrice = 19.20M, CreatedDate = DateTime.Now, Active = true, CategoryId = 1},
            new Product(){ Id = 2, Name = "Coca-cola", Description = "500 ml",UnitPrice = 15M, CreatedDate = DateTime.Now, Active = true, CategoryId = 3}
            };
        }

        private void SetCategories()
        {
            _categories = new List<Category>() {
            new Category(){ Id = 1, Name = "Lácteos",CreatedDate = DateTime.Now},
            new Category(){ Id = 2, Name = "Enlatados",  CreatedDate = DateTime.Now},
            new Category(){ Id = 3, Name = "Gaseosas", CreatedDate = DateTime.Now},
            };
        }

    }
}
