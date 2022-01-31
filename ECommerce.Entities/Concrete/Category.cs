using ECommerce.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Entities.Concrete
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }

        // Relations
        public ICollection<Product> Products { get; set; }
    }
}
