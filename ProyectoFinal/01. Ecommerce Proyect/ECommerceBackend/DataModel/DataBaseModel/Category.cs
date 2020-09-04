using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Category
    {
        public Category()
        {
            ProductByCategory = new HashSet<ProductByCategory>();
        }

        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<ProductByCategory> ProductByCategory { get; set; }
    }
}
