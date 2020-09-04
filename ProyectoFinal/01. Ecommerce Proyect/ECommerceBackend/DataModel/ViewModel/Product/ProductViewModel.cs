using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel.Product
{
    public class ProductViewModel : ProductViewModelBase
    {
        public int Brand { get; set; }
        public CategoryViewModel[] Categories { get; set; }
    }
}
