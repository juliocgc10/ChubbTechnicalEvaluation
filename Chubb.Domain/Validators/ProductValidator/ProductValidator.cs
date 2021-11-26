using Chubb.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Domain.Validators.ProductValidator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("El nombre es requerido").MaximumLength(50).WithMessage("La longitud máxima para el nombre es de 50");
            RuleFor(x => x.Description).NotNull().WithMessage("La descripción es requerida").MaximumLength(50).WithMessage("La longitud máxima para el nombre es de 50"); ;
            RuleFor(x => x.CategoryId).NotNull().WithMessage("La categoría es requerida");
        }
    }
}
