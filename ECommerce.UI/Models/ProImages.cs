using ECommerce.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Models
{
    public class ProImages
    {
        public List<IFormFile> Images { get; set; }
        //public string ProductName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
