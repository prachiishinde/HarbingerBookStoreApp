using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;
using ProductMSystem.Models.ViewModel;

namespace ProductMSystem.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: Index
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _adminService.GetAllProducts();
            return View(objProductList);
        }

        // GET: Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Add
        [HttpPost]
        public IActionResult Add(Product obj)
        {
            try
            {
                _adminService.AddProduct(obj);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Title", ex.Message);
            }
            return View(obj);
        }

        // GET: Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = _adminService.GetProductById((int)id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            try
            {
                _adminService.UpdateProduct(obj);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Title", ex.Message);
            }
            return View(obj);
        }

        // GET: Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = _adminService.GetProductById((int)id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View("Delete", productFromDb);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            _adminService.DeleteProduct((int)id);
            return RedirectToAction("Index");
        }

        // GET: View
        public IActionResult View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productFromDb = _adminService.GetProductById((int)id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
