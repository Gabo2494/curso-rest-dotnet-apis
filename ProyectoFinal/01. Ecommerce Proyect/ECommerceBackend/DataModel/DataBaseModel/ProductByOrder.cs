using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class ProductByOrder
    {
        public int IdProductByOrder { get; set; }
        public int FkProduct { get; set; }
        public int FkOrder { get; set; }

        public virtual Order FkOrderNavigation { get; set; }
        public virtual Product FkProductNavigation { get; set; }
    }
}
