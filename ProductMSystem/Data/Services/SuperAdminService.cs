using Microsoft.AspNetCore.Identity;
using ProductMSystem.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMSystem.Data.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SuperAdminService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityUser>> GetAdminUsersAsync()
        {
            return await _userManager.GetUsersInRoleAsync("Admin");
        }

        public async Task<bool> CreateAdminAsync(AdminRegisterViewModel registerModel)
        {
            var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return true;
            }

            return false;
        }

        public async Task<IdentityUser> GetAdminUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateAdminAsync(IdentityUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Email = user.Email;
            existingUser.UserName = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsersAsync()
        {
            return await _userManager.GetUsersInRoleAsync("User");
        }

        public async Task<bool> CreateUserAsync(CreateUserViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return true;
            }

            return false;
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(IdentityUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Email = user.Email;
            existingUser.UserName = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> PromoteUserToAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                return true;
            }

            return false;
        }
    }

}
