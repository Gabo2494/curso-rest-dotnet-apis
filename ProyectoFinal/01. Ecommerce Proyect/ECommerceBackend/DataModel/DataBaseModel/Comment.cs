using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Comment
    {
        public int IdComment { get; set; }
        public int FkOrder { get; set; }
        public int FkProduct { get; set; }
        public string Description { get; set; }
        public bool Like { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

        public virtual Order FkOrderNavigation { get; set; }
        public virtual Product FkProductNavigation { get; set; }
    }
}
