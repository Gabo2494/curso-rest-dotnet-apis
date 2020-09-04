using DataModel.ViewModel;
using DataModel.ViewModel.Product;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class ProductRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }
        public ApplicationSettings applicationSettings { get; set; }

        public ProductRepository(ECommerceDBContext dbContext, ApplicationSettings applicationSettings)
        {
            UnitOfWork = dbContext;
            this.applicationSettings = applicationSettings;
        }

        public ProductViewModel[] Get()
        {
            var productsQuery = UnitOfWork.Product.Where(p => p.Active).OrderByDescending(p => p.IdProduct)
                .Join(
                UnitOfWork.Brand.Where(p => p.Active),
                x => x.FkBrand,
                y => y.IdBrand,
                (product, brand) => new ProductViewModel()
                {
                    Id = product.IdProduct,
                    Name = product.Name,
                    Price = product.Price,
                    Brand = brand.IdBrand,
                    Color = product.Color,
                    Image = applicationSettings.SendImage ? product.Image : "",
                    Categories = UnitOfWork.ProductByCategory.Where( x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel() 
                    {
                            Id = category.FkCategoryNavigation.IdCategory,
                            Name = category.FkCategoryNavigation.Name,
                            Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : ""
                    }).ToArray()
                }).ToArray();
            return productsQuery;
        }

        public ProductViewModel Get(int id)
        {
            var productsQuery = UnitOfWork.Product.Where(p => p.Active && p.IdProduct == id)
                    .Join(
                    UnitOfWork.Brand.Where(p => p.Active),
                    x => x.FkBrand,
                    y => y.IdBrand,
                    (product, brand) => new ProductViewModel()
                    {
                        Id = product.IdProduct,
                        Name = product.Name,
                        Price = product.Price,
                        Brand = brand.IdBrand,
                        Color = product.Color,
                        Image = applicationSettings.SendImage ? product.Image : "",
                        Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                        {
                            Id = category.FkCategoryNavigation.IdCategory,
                            Name = category.FkCategoryNavigation.Name,
                            Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                        }).ToArray()
                    }).FirstOrDefault();
            return productsQuery;
        }

        public int Save(NewProductViewModel data)
        {
            Brand brand = UnitOfWork.Brand.Find(data.BrandID);
            if (brand == null || !brand.Active)
                return -1;

            var model = new Product
            {
                Name = data.Name,
                Color = data.Color,
                Price = data.Price,
                Image = data.Image,
                FkBrand = brand.IdBrand,
                Active = true
            };

            UnitOfWork.Set<Product>().Add(model);
            UnitOfWork.SaveChanges();

            foreach (var item in data.CategoriesID)
            {
                Category categoyTemp = UnitOfWork.Category.Find(item);
                if (categoyTemp != null)
                {
                    var productCategory = new ProductByCategory
                    {
                        FkProduct = model.IdProduct,
                        FkCategory = categoyTemp.IdCategory
                    };

                    UnitOfWork.Set<ProductByCategory>().Add(productCategory);
                    UnitOfWork.SaveChanges();
                }
            }
            return model.IdProduct;
        }

        public bool Update(int id, NewProductViewModel request)
        {
            Product model = UnitOfWork.Product.Find(id);
            if (model == null || !model.Active)
                return false;
            else
            {
                model.Name = request.Name;
                model.Color = request.Color;
                model.Price = request.Price;
                model.Image = request.Image;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            Product model = UnitOfWork.Product.Find(id);
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

        public ProductViewModel[] GetProductsByCategory(int id)
        {
            var productsQuery = UnitOfWork.ProductByCategory
                .Where(p => p.FkCategory == id && p.FkCategoryNavigation.Active)
                .Select(p => p.FkProductNavigation)
                .Where(p => p.Active)
                .Select(product => new ProductViewModel()
                {
                    Id = product.IdProduct,
                    Name = product.Name,
                    Price = product.Price,
                    Brand = product.FkBrand,
                    Color = product.Color,
                    Image = applicationSettings.SendImage ? product.Image : "",
                    Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                    {
                        Id = category.FkCategoryNavigation.IdCategory,
                        Name = category.FkCategoryNavigation.Name,
                        Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                    }).ToArray()
                }).ToArray();
            return productsQuery;
        }

        public ProductViewModel[] GetProductsByOrder(int id)
        {
            var productsQuery = UnitOfWork.ProductByOrder
                .Where(p => p.FkOrder == id && p.FkOrderNavigation.Active)
                .Select(p => p.FkProductNavigation)
                .Select(product => new ProductViewModel()
                {
                    Id = product.IdProduct,
                    Name = product.Name,
                    Price = product.Price,
                    Brand = product.FkBrand,
                    Color = product.Color,
                    Image = applicationSettings.SendImage ? product.Image : "",
                    Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                    {
                        Id = category.FkCategoryNavigation.IdCategory,
                        Name = category.FkCategoryNavigation.Name,
                        Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                    }).ToArray()
                }).ToArray();
            return productsQuery;
        }

        public ProductViewModel[] GetProductsByWord(string word)
        {
            var productsQuery = UnitOfWork.Product
                .Where(p => p.Active && (p.Name.Contains(word) || p.Color.Contains(word) || p.FkBrandNavigation.Name.Contains(word)))
                .Join(
                UnitOfWork.Brand.Where(p => p.Active),
                x => x.FkBrand,
                y => y.IdBrand,
                (product, brand) => new ProductViewModel()
                {
                    Id = product.IdProduct,
                    Name = product.Name,
                    Price = product.Price,
                    Brand = brand.IdBrand,
                    Color = product.Color,
                    Image = applicationSettings.SendImage ? product.Image : "",
                    Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                    {
                        Id = category.FkCategoryNavigation.IdCategory,
                        Name = category.FkCategoryNavigation.Name,
                        Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                    }).ToArray()
                }).ToArray();
            return productsQuery;
        }

        public ProductViewModel[] GetTop10New()
        {
            var products = UnitOfWork.Product.Where(p => p.Active).OrderByDescending(p => p.IdProduct)
                .Join(
                UnitOfWork.Brand.Where(p => p.Active),
                x => x.FkBrand,
                y => y.IdBrand,
                (product, brand) => new ProductViewModel()
                {
                    Id = product.IdProduct,
                    Name = product.Name,
                    Price = product.Price,
                    Brand = brand.IdBrand,
                    Color = product.Color,
                    Image = applicationSettings.SendImage ? product.Image : "",
                    Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                    {
                        Id = category.FkCategoryNavigation.IdCategory,
                        Name = category.FkCategoryNavigation.Name,
                        Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                    }).ToArray()
                });

            if (products.Count() > 10)
                return products.Take(10).ToArray();
            else
                return products.ToArray();
        }

        public ProductViewModel[] GetTop10()
        {
            var products = UnitOfWork.ProductByOrder
                .Select(p => p.FkProductNavigation).ToArray();

            var groups = products.GroupBy(p => p.IdProduct).ToArray();

            var productsResp = groups
            .Select(p => p.ToList().First())
            .OrderByDescending(p => p.IdProduct)
            .Select(product => new ProductViewModel()
            {
                Id = product.IdProduct,
                Name = product.Name,
                Price = product.Price,
                Brand = product.FkBrand,
                Color = product.Color,
                Image = applicationSettings.SendImage ? product.Image : "",
                Categories = UnitOfWork.ProductByCategory.Where(x => x.FkProduct == product.IdProduct).Select(category => new CategoryViewModel()
                {
                    Id = category.FkCategoryNavigation.IdCategory,
                    Name = category.FkCategoryNavigation.Name,
                    Image = applicationSettings.SendImage ? category.FkCategoryNavigation.Image : "",
                }).ToArray()
            }).ToArray();

            if (productsResp.Count() > 10)
                return productsResp.Take(10).ToArray();
            else
                return productsResp;


        }

    }
}
