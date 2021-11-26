using Chubb.Domain.Common;
using Chubb.Domain.DTOs;
using Chubb.Domain.Entities;
using Chubb.Domain.Extensions;
using Chubb.Domain.Interfaces;
using Chubb.Domain.Validators.ProductValidator;
using Chubb.Resources;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chubb.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISchoolContext schoolContext;
        private readonly IProductRepository productRepository;

        public ProductController(ISchoolContext schoolContext, IProductRepository productRepository)
        {
            this.schoolContext = schoolContext;
            this.productRepository = productRepository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get()
        {
            ResponseServices response = new ResponseServices();
            try
            {
                response.Data = productRepository.GetAll();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.AddException(ex);
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            ResponseServices response = new ResponseServices();
            try
            {                

                Product product = productRepository.GetBy(x => x.Id == id);

                if (product != null)
                {
                    response.Data = product;
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
                response.AddException(ex);
                return BadRequest(response);
            }

            return Ok(response);
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post(ProductAddDto productDto)
        {
            ResponseServices response = new ResponseServices();
            try
            {               
                Product product = new Product()
                {
                    Name = productDto.Name,
                    UnitPrice = productDto.UnitPrice,
                    Active = productDto.Active,
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
                response.AddException(ex);
                return BadRequest(response);
            }

            return Ok(response);

        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductUpdateDto productDto)
        {
            ResponseServices response = new ResponseServices();
            try
            {
                var product = productRepository.GetBy(x => x.Id == id);
                if (product != null)
                {
                    product.Name = productDto.Name;
                    product.Description = productDto.Description;
                    product.UnitPrice = productDto.UnitPrice;
                    product.Active = productDto.Active;
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

            }
            catch (Exception ex)
            {
                response.AddException(ex);
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ResponseServices response = new ResponseServices();
            try
            {
                response.Data = productRepository.Delete(id);

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.AddException(ex);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
