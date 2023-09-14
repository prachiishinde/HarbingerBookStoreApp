using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductMSystem.Data.Services;
using ProductMSystem.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminService _superAdminService;

        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> SuperAdminDashboard()
        {
            var adminUsers = await _superAdminService.GetAdminUsersAsync();
            return View("SuperAdminDashboard",adminUsers);
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        //Create Admin
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminRegisterViewModel registerModel)
        {
            var result = await _superAdminService.CreateAdminAsync(registerModel);
            if (result)
            {
                return RedirectToAction("SuperAdminDashboard");
            }


            ModelState.AddModelError(string.Empty, "Failed to create admin.");
            return View(registerModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditAdmin(string id)
        {
            var user = await _superAdminService.GetAdminUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new IdentityUser
            {
                Id = user.Id,
                Email = user.Email,
            };

            return View("EditAdmin", model);
        }

        //Edit Admin
        [HttpPost]
        public async Task<IActionResult> EditAdmin(IdentityUser user)
        {
            var result = await _superAdminService.UpdateAdminAsync(user);
            if (result)
            {
                return RedirectToAction("SuperAdminDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to update admin.");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var user = await _superAdminService.GetAdminUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View("DeleteAdmin", user);
        }

        //Delete Admin
        [HttpPost]
        public async Task<IActionResult> DeleteAdminConfirmed(string id)
        {
            var result = await _superAdminService.DeleteAdminAsync(id);
            if (result)
            {
                return RedirectToAction("SuperAdminDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete admin.");
            return View(id);
        }

        [HttpGet]
        public async Task<IActionResult> UserDashboard()
        {
            var users = await _superAdminService.GetUsersAsync();
            return View("UserDashboard",users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        //Create user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            var result = await _superAdminService.CreateUserAsync(model);
            if (result)
            {
                return RedirectToAction("UserDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to create user.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _superAdminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new IdentityUser
            {
                Id = user.Id,
                Email = user.Email,
            };

            return View(model);
        }

        //Edit user
        [HttpPost]
        public async Task<IActionResult> EditUser(IdentityUser user)
        {
            var result = await _superAdminService.UpdateUserAsync(user);
            if (result)
            {
                return RedirectToAction("UserDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to update user.");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _superAdminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //Delete user
        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var result = await _superAdminService.DeleteUserAsync(id);
            if (result)
            {
                return RedirectToAction("UserDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete user.");
            return View(id);
        }

        //Make user as admin
        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            var result = await _superAdminService.PromoteUserToAdminAsync(userId);
            if (result)
            {
                return RedirectToAction("UserDashboard");
            }

            ModelState.AddModelError(string.Empty, "Failed to promote user to admin.");
            return RedirectToAction("SuperAdminDashboard");
        }

        public IActionResult MainDashboard()
        {
            return View();
        }
    }

}
