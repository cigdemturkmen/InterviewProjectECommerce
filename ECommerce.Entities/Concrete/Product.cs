using ECommerce.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Entities.Concrete
{
    public class Product: BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; } 

        public ICollection<ProductImage> ProductImages { get; set; }

        // Relations
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
