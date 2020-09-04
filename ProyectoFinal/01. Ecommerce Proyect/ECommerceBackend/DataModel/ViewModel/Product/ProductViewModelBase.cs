using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel.Product
{
    public class ProductViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
    
    }
}
