using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
    }
}
