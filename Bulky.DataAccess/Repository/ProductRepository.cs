using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product entity)
        {
            //_db.Products.Update(entity);
            // Manual Update -> We can implement specfic business logic

            var obj = _db.Products.FirstOrDefault(u=>u.Id == entity.Id);
            if(obj != null)
            {
                obj.Title = entity.Title;
                obj.ISBN = entity.ISBN;
                obj.Description = entity.Description;
                obj.Author = entity.Author;
                obj.ListPrice = entity.ListPrice;
                obj.Price = entity.Price;
                obj.Price50 = entity.Price50;
                obj.Price100 = entity.Price100;
                obj.CategoryId = entity.CategoryId;
                if(entity.ImageUrl != null)
                {
                    obj.ImageUrl = entity.ImageUrl;
                }
            }

        }

    }
}
