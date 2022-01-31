using ECommerce.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
