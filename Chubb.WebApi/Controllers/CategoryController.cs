using Chubb.Domain.Common;
using Chubb.Domain.DTOs;
using Chubb.Domain.Extensions;
using Chubb.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chubb.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(IChubbContext schoolContext, ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get(string searchText)
        {
            ResponseServices response = new ResponseServices();
            try
            {

                response.Data = categoryRepository.GetAll().Select(x => new CategoryDto()
                {
                    Id = x.Id,
                    Name = x.Name
                });

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
