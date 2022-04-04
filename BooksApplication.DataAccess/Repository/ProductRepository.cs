using BooksApplication.DataAccess.Repository.IRepository;
using BooksApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApplication.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private  ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
            _db = db; 
        }

        public void Update(Product obj)
        {
            var objFromdb = _db.Products.Where(x => x.ProductId == obj.ProductId).FirstOrDefault();
            if (objFromdb != null)
            {
                objFromdb.ISBN = obj.ISBN;
                objFromdb.Author = obj.Author;
                objFromdb.Description = obj.Description;
                objFromdb.Price = obj.Price;
                objFromdb.Price50 = obj.Price50;
                objFromdb.Price100 = obj.Price100;
                objFromdb.ListPrice = obj.ListPrice;
                objFromdb.Title = obj.Title;
                objFromdb.CategoryId = obj.CategoryId;
                objFromdb.CoverTypeId = obj.CoverTypeId;
                if(obj.ImageUrl != null)
                {
                    objFromdb.ImageUrl = obj.ImageUrl;
                }
                
            }
        }
    }
}
