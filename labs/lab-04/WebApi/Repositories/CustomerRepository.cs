using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastructure.Data.Models;
using WebApi.ViewModels;
using FluentAssertions;

namespace WebApi.Repositories
{
    public class CustomerRepository
    {
        public AdventureworksContext UnitOfWork { get; set; }

        public CustomerRepository(AdventureworksContext dbContext)
        {
            UnitOfWork = dbContext;
        }

        public object[] Get()
        {
            return UnitOfWork.Customer
                .Select(p => new
                {
                    Name = p.FirstName,
                    p.LastName,
                    Company = p.CompanyName,
                    p.Phone,
                    Email = p.EmailAddress
                })
                .ToArray();
        }

        internal object Get(int id)
        {
            var query = UnitOfWork.Customer
                .Where(p => p.CustomerId == id)
                .Select(p => new
                {
                    Name = p.FirstName,
                    p.LastName,
                    Company = p.CompanyName,
                    p.Phone,
                    Email = p.EmailAddress
                })
                .FirstOrDefault();
            return query;
        }

        public int Save(CustomerViewModel data)
        {
            var query = UnitOfWork.Set<Customer>().AsQueryable();


            var model = new Customer
            {
                CompanyName = data.Company,
                EmailAddress = data.Email,
                LastName = data.LastName,
                FirstName = data.Name,
                Phone = data.Phone,
                PasswordHash = "123456789",
                PasswordSalt = "123456789"
            };

            UnitOfWork.Set<Customer>().Add(model);

            UnitOfWork.SaveChanges();

            return model.CustomerId;
        }

        internal bool Update(int id, CustomerViewModel request)
        {
            Customer model = UnitOfWork.Customer.Find(id);
            if (model == null)
                return false;
            else
            {
                model.FirstName = request.Name;
                model.LastName = request.LastName;
                model.Phone = request.Phone;
                model.EmailAddress = request.Email;
                model.CompanyName = request.Company;

                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();

                return true;
            }
        }

        public bool Delete(int id)
        {
            Customer model = UnitOfWork.Customer.Find(id);
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
