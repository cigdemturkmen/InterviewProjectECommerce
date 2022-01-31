using ECommerce.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Entities.Concrete
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(15)]
        public string UserRole { get; set; }

        // Relations
        public List<User> Users { get; set; }
    }
}
