using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public partial class PaymentData
    {
        public PaymentData()
        {
            Order = new HashSet<Order>();
        }

        public int IdPaymentData { get; set; }
        public string LastDigitCard { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
