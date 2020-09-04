using DataModel.Authentication;
using DataModel.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel.Order
{
    public class NewOrderViewModel 
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int[] ProductsID { get; set; }
        public DateTime CreationDate { get; set; }
        public AuthorizationModel User { get; set; }
        public PaymentData PaymentData { get; set; }
    }
}
