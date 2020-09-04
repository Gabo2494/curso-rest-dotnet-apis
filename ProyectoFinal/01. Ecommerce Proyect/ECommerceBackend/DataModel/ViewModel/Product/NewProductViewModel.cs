using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModel.ViewModel.Product
{
    public class NewProductViewModel : ProductViewModelBase
    {
        [Required]
        public int BrandID { get; set; }
        [Required]
        public int[] CategoriesID { get; set; }
    }
}
