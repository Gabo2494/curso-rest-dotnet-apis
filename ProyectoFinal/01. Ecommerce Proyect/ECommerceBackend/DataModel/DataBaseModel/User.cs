using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class User
    {
        public User()
        {
            Order = new HashSet<Order>();
            Token = new HashSet<Token>();
        }

        public int IdUser { get; set; }
        public int FkRole { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public bool Active { get; set; }
        public string LastName { get; set; }

        public virtual Role FkRoleNavigation { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Token> Token { get; set; }
    }
}
