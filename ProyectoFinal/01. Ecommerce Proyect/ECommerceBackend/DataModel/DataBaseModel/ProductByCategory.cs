using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class ProductByCategory
    {
        public int IdProductByCategory { get; set; }
        public int FkCategory { get; set; }
        public int FkProduct { get; set; }

        public virtual Category FkCategoryNavigation { get; set; }
        public virtual Product FkProductNavigation { get; set; }
    }
}
