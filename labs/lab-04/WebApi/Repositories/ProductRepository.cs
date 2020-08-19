using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Infrastructure.Data.Models;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class ProductRepository
    {
        public AdventureworksContext UnitOfWork { get; set; }

        public ProductRepository(AdventureworksContext dbContext)
        {
            UnitOfWork = dbContext;
        }

        private readonly List<string> Products= new List<string>()
        {
            "Jeans","T-shirt","Pants"
        };

        //public object[] Get()
        //{
        //    int i = 0;
        //    return Products.Select(model => new
        //    {
        //        Name = model,
        //        Id = i++
        //    }).ToArray();
        //}

        public object[] Get()
        {
            return UnitOfWork.Product
                .Select(p => new
                {
                    Name = p.Name,
                    p.ListPrice,
                    Category = new { p.ProductCategory.Name },
                    p.Weight
                })
                .ToArray();
        }

        //public object Get(int id)
        //{
        //    int i = 0;
        //    var result = Products.Select(model => new
        //    {
        //        Name = model,
        //        Id = i++
        //    }).ToList();

        //    return result.ElementAtOrDefault(id);
        //}

        public object Get(int id)
        {
            var query = UnitOfWork.Product
                .Where(p => p.ProductId == id)
                .Select(p => new
                {
                    Name = p.Name,
                    p.ListPrice,
                    Category = new { p.ProductCategory.Name },
                    p.Weight
                })
                .FirstOrDefault();
            return query;
        }


        public int Save(ProductViewModel data)
        {
            //store in DB
            //Products.Add(name);

            var query = UnitOfWork.Set<Product>().AsQueryable();
            var next = query.Max(p => p.ProductNumber) + 1;

            /*
             Nombre no es requerido
             SellStartDate/SellEndDate debe asignarse
             
             */

            var model = new Product { 
                Name = data.Name,
                SellStartDate = DateTime.Now,
                SellEndDate = DateTime.Now.AddYears(1),
                ProductNumber = next
            };

            UnitOfWork.Set<Product>().Add(model);

            UnitOfWork.SaveChanges();

            return model.ProductId;
        }

        public bool Update(int id, ProductViewModel request)
        {
            Product model = UnitOfWork.Product.Find(id);
            if (model == null)
                return false;
            else
            {
                model.ListPrice = request.ListPrice;
                model.Name = request.Name;
                model.Weight = request.Weight;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();

                return true;
            }
        }

        public bool Delete(int id)
        {
            Product model = UnitOfWork.Product.Find(id);
            if (model == null)
                return false;
            else
            {
                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                UnitOfWork.SaveChanges();

                return true;
            }
        }
    }

    
}