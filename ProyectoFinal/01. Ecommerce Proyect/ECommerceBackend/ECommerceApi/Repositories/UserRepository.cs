using DataModel.Authentication;
using DataModel.ViewModel;
using ECommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApi.Repositories
{
    public class UserRepository
    {
        public ECommerceDBContext UnitOfWork { get; set; }

        public UserRepository(ECommerceDBContext dbContext)
        {
            UnitOfWork = dbContext;
        }

        public UserViewModel[] Get()
        {
            return UnitOfWork.User
                .Where(p => p.Active)
                .Select(p => new UserViewModel()
                {
                     
                    Id = p.IdUser,
                    Name = p.Name,
                    Email = p.Email,
                    Birthday = p.Birthday,
                    LastName = p.LastName
                })
                .ToArray();
        }

        public UserViewModel Get(int id)
        {
            var query = UnitOfWork.User
                .Where(p => p.IdUser == id && p.Active)
                .Select(p => new UserViewModel()
                {
                    Id = p.IdUser,
                    Name = p.Name,
                    Email = p.Email,
                    Birthday = p.Birthday,
                    LastName = p.LastName
                })
                .FirstOrDefault();
            return query;
        }

        public int Save(NewUserViewModel data)
        {
            var role = UnitOfWork.Role
                .Where(p => p.Name == "AppUser")
                .Select(p => new
                {
                    Id = p.IdRole
                })
                .FirstOrDefault();

            if (role != null)
            {
                var model = new User
                {
                    Name = data.Name,
                    Email = data.Email,
                    Password = data.Password,
                    Birthday = data.Birthday,
                    LastName = data.LastName,
                    FkRole = role.Id,
                    Active = true
                };
                UnitOfWork.Set<User>().Add(model);

                UnitOfWork.SaveChanges();

                return model.IdUser;
            }
            return -1;

        }

        public bool Update(int id, NewUserViewModel request)
        {
            User model = UnitOfWork.User.Find(id);
            if (model == null || !model.Active )
                return false;
            else
            {
                model.Name = request.Name;
                model.LastName = request.LastName;
                
                UnitOfWork.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                UnitOfWork.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            User model = UnitOfWork.User.Find(id);
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

        public AuthorizationModel Login(LoginModel data)
        {
            var token = CreateMD5(DateTime.Now.ToString());
            var query = UnitOfWork.User
                .Where(p => p.Email == data.Email && p.Password == data.Password)
                .Select(p => new AuthorizationModel()
                {
                  Name = p.Name,
                   Email =  p.Email,
                   Token = token,
                    Id = p.IdUser
                })
                .FirstOrDefault();

            if (query != null)
            {
                var model = new Token
                {
                     Expiration = DateTime.Now.AddHours(1),
                     FkUser = UnitOfWork.User
                                    .Where(p => p.Email == data.Email)
                                    .FirstOrDefault().IdUser,
                      
                };
                UnitOfWork.Set<Token>().Add(model);

                UnitOfWork.SaveChanges();
            }

            return query;
        }

        private  string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }



    }
}
