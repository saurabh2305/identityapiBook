using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookIdentityAPI.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "username can't be empty")]
        public string Username { get; set; }

        [Required(ErrorMessage = "password can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
