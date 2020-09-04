using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Product
    {
        public Product()
        {
            Comment = new HashSet<Comment>();
            ProductByCategory = new HashSet<ProductByCategory>();
            ProductByOrder = new HashSet<ProductByOrder>();
        }

        public int IdProduct { get; set; }
        public int FkBrand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

        public virtual Brand FkBrandNavigation { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<ProductByCategory> ProductByCategory { get; set; }
        public virtual ICollection<ProductByOrder> ProductByOrder { get; set; }
    }
}
