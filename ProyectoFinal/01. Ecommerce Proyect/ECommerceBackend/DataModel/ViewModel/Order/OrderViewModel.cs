using DataModel.Authentication;
using DataModel.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public ProductViewModelBase[] Products { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
