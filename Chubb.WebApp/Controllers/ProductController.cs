using Chubb.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chubb.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IOptions<AppSettings> options;

        public ProductController(IOptions<AppSettings> options)
        {
            this.options = options;
        }
        public IActionResult Index()
        {
            ViewBag.UrlWebApi = options.Value.StorageConfiguration.UrlWebApi;
            return View();
        }
    }
}
