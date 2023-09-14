using System;
using System.Collections.Generic;
using System.Linq;
using ProductMSystem.Data;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;

namespace ProductMSystem.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;

        public AdminService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                if (product == null)
                {
                    return false; // Product not found, deletion failed
                }

                _db.Products.Remove(product);
                _db.SaveChanges();
                return true; // Deletion successful
            }
            catch (Exception)
            {
                return false; // Exception occurred, deletion failed
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            if (product.Title == product.Author.ToString())
            {
                throw new ArgumentException("The Title cannot exactly match the Author.");
            }

            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            if (product.Title == product.Author.ToString())
            {
                throw new ArgumentException("The Title cannot exactly match the Author.");
            }

            _db.Products.Update(product);
            _db.SaveChanges();
        }

    }


}
