using DataModel.ViewModel;
using DataModel.ViewModel.Product;
using ECommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class CategoryRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }
        public ApplicationSettings applicationSettings { get; set; }
        
        public CategoryRepository(ECommerceDBContext dbContext, ApplicationSettings applicationSettings)
        {
            UnitOfWork = dbContext;
            this.applicationSettings = applicationSettings;
        }

        public CategoryViewModel[] Get()
        {
            return UnitOfWork.Category
                .Where(p => p.Active)
                .Select(p => new CategoryViewModel()
                {
                    Id = p.IdCategory,
                    Name = p.Name,
                    Image = applicationSettings.SendImage ? p.Image : ""
                })
                .ToArray();
        }

        public CategoryViewModel Get(int id)
        {
            var query = UnitOfWork.Category
                .Where(p => p.IdCategory == id && p.Active)
                .Select(p => new CategoryViewModel()
                {
                    Id = p.IdCategory,
                    Name =  p.Name,
                    Image = applicationSettings.SendImage ? p.Image : ""
                })
                .FirstOrDefault();
            return query;
        }

        public int Save(CategoryViewModel data)
        {
            var model = new Category
            {
                Name = data.Name,
                Image = data.Image,
                Active = true
            };

            UnitOfWork.Set<Category>().Add(model);

            UnitOfWork.SaveChanges();

            return model.IdCategory;
        }

        public bool Update(int id, CategoryViewModel request)
        {
            Category model = UnitOfWork.Category.Find(id);
            if (model == null || !model.Active)
                return false;
            else
            {
                model.Name = request.Name;
                model.Image = request.Image;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            Category model = UnitOfWork.Category.Find(id);
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

        public CategoryViewModel[] GetTop10New()
        {
            var categories =  UnitOfWork.Category
                .Where(p => p.Active)
                .OrderByDescending(p => p.IdCategory)
                .Select(p => new CategoryViewModel()
                {
                    Id = p.IdCategory,
                    Name = p.Name,
                    Image = applicationSettings.SendImage ? p.Image : ""
                });

            if (categories.Count() > 10)
                return categories.Take(10).ToArray();
            else
                return categories.ToArray();
        }

    }
}
