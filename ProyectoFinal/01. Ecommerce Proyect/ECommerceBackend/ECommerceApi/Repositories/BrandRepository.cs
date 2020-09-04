using DataModel.ViewModel;
using ECommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class BrandRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }

        public BrandRepository(ECommerceDBContext dbContext)
        {
            UnitOfWork = dbContext;
        }

        public BrandViewModel[] Get()
        {
            return UnitOfWork.Brand
                .Where(p => p.Active)
                .Select(p => new BrandViewModel()
                {
                    Id = p.IdBrand,
                    Name = p.Name
                })
                .ToArray();
        }

        internal BrandViewModel Get(int id)
        {
            var query = UnitOfWork.Brand
                .Where(p => p.IdBrand == id && p.Active)
                .Select(p => new BrandViewModel()
                {
                    Id = p.IdBrand,
                    Name = p.Name
                })
                .FirstOrDefault();
            return query;
        }

        public int Save(BrandViewModel data)
        { 
            var model = new Brand
            {
                Name = data.Name,
                Active = true
            };

            UnitOfWork.Set<Brand>().Add(model);

            UnitOfWork.SaveChanges();

            return model.IdBrand;
        }

        public bool Update(int id, BrandViewModel request)
        {
            Brand model = UnitOfWork.Brand.Find(id);
            if (model == null || !model.Active)
                return false;
            else
            {
                model.Name = request.Name;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            Brand model = UnitOfWork.Brand.Find(id);
            if (model == null || !model.Active)
                return false;
            else
            {
                model.Active = false;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }


    }
}
