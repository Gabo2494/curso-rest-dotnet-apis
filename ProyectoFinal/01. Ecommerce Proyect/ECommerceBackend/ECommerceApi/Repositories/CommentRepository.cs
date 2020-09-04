using DataModel.ViewModel;
using ECommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class CommentRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }
        public ApplicationSettings applicationSettings { get; set; }

        public CommentRepository(ECommerceDBContext dbContext, ApplicationSettings applicationSettings)
        {
            UnitOfWork = dbContext;
            this.applicationSettings = applicationSettings;
        }
        public CommentViewModel[] Get()
        {
            var query = UnitOfWork.Comment
                .Where(p => p.Active)
                .Select(p => new CommentViewModel()
                {
                    Id = p.IdComment,
                    Description = p.Description,
                    Image = applicationSettings.SendImage ? p.Image : "",
                    ProductId = p.FkProduct,
                    Like = p.Like,
                    OrderId = p.FkOrder,
                    User = p.FkOrderNavigation.FkUserNavigation.Email
                })
                .ToArray();
            return query;
        }

        public CommentViewModel Get(int id)
        {
            var query = UnitOfWork.Comment
                .Where(p => p.IdComment == id && p.Active)
                .Select(p => new CommentViewModel()
                {
                    Id = p.IdComment,
                    Description = p.Description,
                    Image = applicationSettings.SendImage ? p.Image : "",
                    ProductId = p.FkProduct,
                    Like = p.Like,
                    OrderId = p.FkOrder,
                    User = p.FkOrderNavigation.FkUserNavigation.Email
                })
                .FirstOrDefault();
            return query;
        }

        public int Save(CommentViewModel data)
        {
            Product product = UnitOfWork.Product.Find(data.ProductId);
            Order order = UnitOfWork.Order.Find(data.OrderId);
            if (product == null || order == null || !product.Active)
                return -1;

            var model = new Comment
            {
                Description = data.Description,
                Image = data.Image ?? "",
                FkProduct = product.IdProduct,
                Like = data.Like,
                FkOrder = order.IdOrder,
                Active = true
            };

            UnitOfWork.Set<Comment>().Add(model);

            UnitOfWork.SaveChanges();

            return model.IdComment;
        }

        public bool Update(int id, CommentViewModel request)
        {
            Comment model = UnitOfWork.Comment.Find(id);
            if (model == null || !model.Active)
                return false;
            else
            {
                model.Description = request.Description;
                model.Like = request.Like;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            Comment model = UnitOfWork.Comment.Find(id);
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

        public CommentViewModel[] GetCommentByProduct(int id)
        {
            var comments = UnitOfWork.Comment
                .Where(p => p.Active && p.FkProduct == id)
                .Select(p => new CommentViewModel()
                {
                    Id = p.IdComment,
                    Description = p.Description,
                    Image = p.Image,
                    ProductId = p.FkProduct,
                    Like = p.Like,
                    OrderId = p.FkOrder,
                    User = p.FkOrderNavigation.FkUserNavigation.Email
                })
                .ToArray();
            return comments;
        }
    }
}
