using ProductMSystem.Models;

namespace ProductMSystem.Data.Services
{
    public interface IUserService
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProducts();
    }
}
