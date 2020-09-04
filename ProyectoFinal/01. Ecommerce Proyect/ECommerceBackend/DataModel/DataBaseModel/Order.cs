using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class Order
    {
        public Order()
        {
            Comment = new HashSet<Comment>();
            ProductByOrder = new HashSet<ProductByOrder>();
        }

        public int IdOrder { get; set; }
        public int FkPaymentData { get; set; }
        public int FkUser { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Active { get; set; }

        public virtual PaymentData FkPaymentDataNavigation { get; set; }
        public virtual User FkUserNavigation { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<ProductByOrder> ProductByOrder { get; set; }
    }
}
