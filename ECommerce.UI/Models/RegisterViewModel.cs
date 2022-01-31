using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }


    }
}
