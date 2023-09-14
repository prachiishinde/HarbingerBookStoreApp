using Microsoft.AspNetCore.Identity;
using ProductMSystem.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMSystem.Data.Services
{
    public interface ISuperAdminService
    {
        Task<IEnumerable<IdentityUser>> GetAdminUsersAsync();
        Task<bool> CreateAdminAsync(AdminRegisterViewModel registerModel);
        Task<IdentityUser> GetAdminUserByIdAsync(string id);
        Task<bool> UpdateAdminAsync(IdentityUser user);
        Task<bool> DeleteAdminAsync(string id);
        Task<IEnumerable<IdentityUser>> GetUsersAsync();
        Task<bool> CreateUserAsync(CreateUserViewModel model);
        Task<IdentityUser> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(IdentityUser user);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> PromoteUserToAdminAsync(string userId);
    }

}

