using ProductMSystem.Models;

namespace ProductMSystem.Data.Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public Product GetProduct(int id)
        {
            return _db.Products.Find(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return (IEnumerable<Product>)_db.Products.ToList();
        }
    }
}
