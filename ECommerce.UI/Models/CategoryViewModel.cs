using ECommerce.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

        // Relations
        public ICollection<Product> Products { get; set; }
    }
}
