using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModel.Authentication
{
    public class RegistrationModel : UserViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
