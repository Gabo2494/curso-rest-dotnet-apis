using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel.Order
{
    public class PaymentData
    {
        public string Card { get; set; }
        public string Expiration { get; set; }
        public string CCV { get; set; }

    }
}
