using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMSystem.Data;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;
using System.Data;

namespace ProductMSystem.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _userService.GetProduct(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult ViewProduct()
        {
            var productList = _userService.GetAllProducts();
            return View(productList);
        }

        public IActionResult UserDashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
