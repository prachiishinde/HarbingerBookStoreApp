using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ProductMSystem.Models;
using ProductMSystem.Models.ViewModel;

namespace ProductMSystem.Data.Services
{
    public interface IAdminService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        bool DeleteProduct(int id); 

    }
}

