using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookIdentityAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "username can't be empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "password can't be empty")]
        [MinLength(8,ErrorMessage ="password should be minimum of 8 charecters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email can't be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string UserType { get; set; }

        public bool Status { get; set; }

        public string Address { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
