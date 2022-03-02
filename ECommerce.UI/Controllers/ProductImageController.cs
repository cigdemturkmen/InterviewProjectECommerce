using ECommerce.Data.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Controllers
{
    public class ProductImageController : BaseController
    {
        private readonly IRepository<ProductImage> _productImageRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductImageController(IRepository<ProductImage> productImageRepository, IRepository<Product> productRepository, IWebHostEnvironment hostingEnvironment)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _hostingEnvironment = hostingEnvironment;   
        }

        public IActionResult List()
        {
            var productImages = _productImageRepository.GetAll(x => x.IsActive, include: x => x.Include(y => y.Product)).ToList();

            return View(productImages);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productImage = _productImageRepository.Get(x => x.IsActive && x.Id == id, include: x => x.Include(y => y.Product));

            if (productImage != null)
            {
                return View(productImage);
            }

            return NotFound();
        }

        [Authorize(Roles ="1")]
        public IActionResult Add(int id)
        {
            var product = _productRepository.Get(x => x.Id == id && x.IsActive, include: x => x.Include(y => y.ProductImages));

            if (product != null)
            {
                var vm = new ProImages()
                {
                    ProductId = product.Id, 
                    Product = product,
                };

                return View(vm);
            }

            TempData["Message"] = "Product cannot be found!";
            return RedirectToAction("List","Product");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProImages vm)
        { 
            var currentUserId = GetCurrentUserId();

            foreach (var image in vm.Images)
            {
                string stringFileName = UploadFile(image);
                var productImage = new ProductImage
                {
                    Image = stringFileName,
                    ProductId = vm.ProductId,
                    Product = vm.Product,
                    CreatedDate = DateTime.Now,
                    CreatedById = currentUserId,
                };
                _productImageRepository.Add(productImage);
            }

            return RedirectToAction("List", "Product");
        }

        private string UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return fileName;
        }
    }
}
