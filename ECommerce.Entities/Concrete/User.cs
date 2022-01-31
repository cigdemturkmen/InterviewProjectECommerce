using ECommerce.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Entities.Concrete
{
    public class User: BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(12)]
        [MinLength(4)]
        public string Password { get; set; }

        // Relations
        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
