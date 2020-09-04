using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Product = new HashSet<Product>();
        }

        public int IdBrand { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
