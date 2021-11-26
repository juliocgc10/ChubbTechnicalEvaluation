using Chubb.Domain.Common;
using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using Chubb.Domain.Extensions;
using Chubb.Domain.Interfaces;
using Chubb.Domain.Validators.ProductValidator;
using Chubb.Resources;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chubb.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<IProductRepository> logger;

        public ProductController(IProductRepository productRepository, ILogger<IProductRepository> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los productos con el parámetro de búsqueda de nombre, descripción o categoría
        /// </summary>
        /// <param name="searchText">Parámetro de búsqueda de nombre, descripción o categoría</param>
        /// <returns></returns>
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get(string searchText)
        {
            ResponseServices response = new();
            try
            {                
                IEnumerable<Product> products = string.IsNullOrWhiteSpace(searchText) ? productRepository.GetAll() : productRepository.GetAll(x =>
                    x.Name.ToUpper().Contains(searchText.Trim().ToUpper()) ||
                    x.Description.ToUpper().Contains(searchText.Trim().ToUpper()) ||
                    x.Category.Name.ToUpper().Contains(searchText.Trim().ToUpper())
                    );

                response.Data = products.Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryId = x.CategoryId,
                    UnitPrice = x.UnitPrice,
                    CategoryName = x.Category.Name
                });

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                logger.LogCritical(response.AddException(ex));
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtiene un producto con base al identificador ingresado
        /// </summary>
        /// <param name="id">identificador del producto</param>
        /// <returns></returns>
        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            ResponseServices response = new();
            try
            {

                Product product = productRepository.GetBy(x => x.Id == id);

                if (product != null)
                {
                    response.Data = new ProductDto()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        UnitPrice = product.UnitPrice
                    };
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo encontrar el producto, consúltelo con su administrador.";
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(response.AddException(ex));
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Agrega un producto al repositorio
        /// </summary>
        /// <param name="productDto">Producto a agregarse</param>
        /// <returns></returns>
        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post(ProductAddDto productDto)
        {
            ResponseServices response = new();
            try
            {
                Product product = new Product()
                {
                    Name = productDto.Name,
                    UnitPrice = productDto.UnitPrice,
                    CategoryId = productDto.CategoryId,
                    Description = productDto.Description,
                    CreatedDate = DateTime.Now
                };

                ProductValidator validator = new ProductValidator();
                ValidationResult validationResult = validator.Validate(product);

                if (validationResult.IsValid)
                {
                    productRepository.Add(product);
                    response.IsSuccess = true;
                }
                else
                {
                    return Problem(validationResult.ToString("-"));
                }


            }
            catch (Exception ex)
            {
                logger.LogCritical(response.AddException(ex));
                return BadRequest(response);
            }

            return Ok(response);

        }

        /// <summary>
        /// Actualiza un prodcuto en el repositorio
        /// </summary>
        /// <param name="id">Identificador del producto</param>
        /// <param name="productDto">Producto a actualizar</param>
        /// <returns></returns>
        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductUpdateDto productDto)
        {
            ResponseServices response = new();
            try
            {
                var product = productRepository.GetBy(x => x.Id == id);
                if (product != null)
                {
                    product.Name = productDto.Name;
                    product.Description = productDto.Description;
                    product.UnitPrice = productDto.UnitPrice;
                    product.CategoryId = productDto.CategoryId;

                    ProductValidator validator = new ProductValidator();
                    ValidationResult validationResult = validator.Validate(product);

                    if (validationResult.IsValid)
                    {
                        //Queda esta funcionalidad esta de más porque como esta en memoria automaticamente hace la actulización
                        //Pero se mantiene debido a que esta de momento en moeria pero si se cambia el repositorio para que sea por BD sólo cambia la funcionalidad de las clases de repositorio
                        response.Data = productRepository.Update(product);
                        response.IsSuccess = true;
                    }
                    else
                    {
                        return Problem(validationResult.ToString("-"));
                    }

                    response.IsSuccess = true;
                }
                else
                {
                    return Problem("No se encontró el producto a actualizar");
                }


            }
            catch (Exception ex)
            {
                logger.LogCritical(response.AddException(ex));
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Elimina un producto del repositorio
        /// </summary>
        /// <param name="id">Identificador del producto a eliminar</param>
        /// <returns></returns>
        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ResponseServices response = new();
            try
            {

                var product = productRepository.GetBy(x => x.Id == id);
                if (product != null)
                {
                    response.Data = productRepository.Delete(id);
                    response.IsSuccess = true;
                }
                else
                {
                    return Problem("No existe el producto para eliminarse");
                }

            }
            catch (Exception ex)
            {
                
                logger.LogCritical(response.AddException(ex));
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
