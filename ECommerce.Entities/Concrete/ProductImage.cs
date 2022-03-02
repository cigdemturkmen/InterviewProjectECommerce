using ECommerce.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Entities.Concrete
{
    public class ProductImage: BaseEntity
    {
        
        public string Image { get; set; }

        // Relations
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }


    }
}
