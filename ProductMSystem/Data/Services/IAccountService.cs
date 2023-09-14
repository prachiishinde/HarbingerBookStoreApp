using Microsoft.AspNetCore.Identity;
using ProductMSystem.Models.ViewModel;

namespace ProductMSystem.Data.Services
{
    public interface IAccountService
    {
        Task<(bool Succeeded, IEnumerable<string> Roles)> LoginAsync(string email, LoginViewModel model);

        Task<IdentityResult> RegisterAsync(RegisterViewModel registerModel);
        Task SignOutAsync();
        Task<IdentityUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<string>> GetRolesAsync(IdentityUser user);
    }
}


