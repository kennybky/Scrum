using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;
using Scrum.Models;
using Scrum.Services;

namespace Scrum.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAuthorizationService _authorizationService;

        private readonly ScrumContext _dbContext;
        public HomeController(IAuthorizationService authorizationService, ScrumContext dbContext)
        {
            _authorizationService = authorizationService;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Test()
        {
            
            var Product = _dbContext.Products.Include(P => P.ProductManager).ToList().FirstOrDefault();

            if (Product == null)
               return RedirectToAction("Error");
            var User = HttpContext.User;
            
            var authorization = await _authorizationService.AuthorizeAsync(User, Product, Operations.Manage);
            if (authorization.Succeeded)
            {
                return RedirectToAction("About");
            } else
            {
                return RedirectToAction("Privacy");
            }
        }
    }
}
