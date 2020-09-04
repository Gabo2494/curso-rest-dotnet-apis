using DataModel.ViewModel.Order;
using DataModel.ViewModel.Product;
using ECommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class OrderRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }
        public ApplicationSettings applicationSettings { get; set; }

        public OrderRepository(ECommerceDBContext dbContext, ApplicationSettings applicationSettings)
        {
            UnitOfWork = dbContext;
            this.applicationSettings = applicationSettings;
        }

        public OrderViewModel[] Get()
        {
            var listOrder = UnitOfWork.Order
                .Where(p => p.Active)
                .Select(p => new OrderViewModel()
                {
                    Id = p.IdOrder,
                    CreationDate = p.CreationDate,
                    TotalAmount = p.TotalAmount,
                    Products = UnitOfWork.ProductByOrder
                               .Where(q => q.FkOrder == p.IdOrder)
                               .Select(q => new ProductViewModel()
                               {
                                   Id = q.FkProductNavigation.IdProduct,
                                   Name = q.FkProductNavigation.Name,
                                   Color = q.FkProductNavigation.Color,
                                   Price = q.FkProductNavigation.Price,
                                   Image = "",//q.FkProductNavigation.Image,
                                   Brand = q.FkProductNavigation.FkBrandNavigation.IdBrand
                               }).ToArray()
                }).ToArray();

            return listOrder;
        }
        
        public OrderViewModel Get(int id)
        {
            var order = UnitOfWork.Order
                .Where(p => p.Active && p.IdOrder == id)
                .Select(p => new OrderViewModel()
                {
                    Id = p.IdOrder,
                    CreationDate = p.CreationDate,
                    TotalAmount = p.TotalAmount,
                    Products = UnitOfWork.ProductByOrder
                               .Where(q => q.FkOrder == p.IdOrder)
                               .Select(q => new ProductViewModel()
                               {
                                   Id = q.FkProductNavigation.IdProduct,
                                   Name = q.FkProductNavigation.Name,
                                   Color = q.FkProductNavigation.Color,
                                   Price = q.FkProductNavigation.Price,
                                   Image = applicationSettings.SendImage ? q.FkProductNavigation.Image : "", 
                                   Brand = q.FkProductNavigation.FkBrandNavigation.IdBrand
                               }).ToArray()
                }).FirstOrDefault();

            return order;
        }

        public int Save(NewOrderViewModel data)
        {
            User user = UnitOfWork.User.Where(p => p.Email == data.User.Email).FirstOrDefault();
            if (user == null || !user.Active)
                return -1;

            Models.PaymentData paymentModel = CreatePayment(data);

            var orderModel = new Order
            {
                CreationDate = DateTime.Now,
                TotalAmount = data.TotalAmount,
                FkUser = user.IdUser,
                FkPaymentData = paymentModel.IdPaymentData,
                Active = true
            };

            UnitOfWork.Set<Order>().Add(orderModel);
            UnitOfWork.SaveChanges();

            foreach (var item in data.ProductsID)
            {
                Product productTemp = UnitOfWork.Product.Find(item);
                if (productTemp != null)
                {
                    var productByOrder = new ProductByOrder
                    {
                        FkProduct = productTemp.IdProduct,
                        FkOrder = orderModel.IdOrder
                    };

                    UnitOfWork.Set<ProductByOrder>().Add(productByOrder);
                    UnitOfWork.SaveChanges();
                }
            }
            return orderModel.IdOrder;
        }

        public bool Delete(int id)
        {
            Order model = UnitOfWork.Order.Find(id);
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

        private Models.PaymentData CreatePayment(NewOrderViewModel data)
        {
            var paymentModel = new Models.PaymentData
            {
                Amount = data.TotalAmount,
                Date = DateTime.Now,
                LastDigitCard = data.PaymentData.Card.Substring(data.PaymentData.Card.Length - 5, 4)
            };
            UnitOfWork.Set<Models.PaymentData>().Add(paymentModel);
            UnitOfWork.SaveChanges();
            return paymentModel;
        }

        public OrderViewModel[] GetOrderByUser(int idUser)
        {
            var listOrder = UnitOfWork.Order
                .Where(p => p.Active && p.FkUser == idUser)
                .Select(p => new OrderViewModel()
                {
                    Id = p.IdOrder,
                    CreationDate = p.CreationDate,
                    TotalAmount = p.TotalAmount,
                    Products = UnitOfWork.ProductByOrder
                               .Where(q => q.FkOrder == p.IdOrder)
                               .Select(q => new ProductViewModel()
                               {
                                   Id = q.FkProductNavigation.IdProduct,
                                   Name = q.FkProductNavigation.Name,
                                   Color = q.FkProductNavigation.Color,
                                   Price = q.FkProductNavigation.Price,
                                   Image = "",//q.FkProductNavigation.Image,
                                   Brand = q.FkProductNavigation.FkBrandNavigation.IdBrand
                               }).ToArray()
                }).ToArray();

            return listOrder;
        }
    }
}
