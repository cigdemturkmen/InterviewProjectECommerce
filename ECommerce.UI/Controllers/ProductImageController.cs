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

        public ProductImageController(IRepository<ProductImage> productImageRepository, IWebHostEnvironment hostingEnvironment, IRepository<Product> productRepository)
        {
            _productImageRepository = productImageRepository;
            _hostingEnvironment = hostingEnvironment;
            _productRepository = productRepository;
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
        public IActionResult Add(int? id)
        {
            ProImages vm = new ProImages();
            //ViewBag.Images = new SelectList(_productImageRepository.GetAll().ToList(), "Id", "Product.ProductName");
            //ViewBag.Products = _productRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            //{
            //    Text = x.ProductName,
            //    Value = x.Id.ToString(),
            //}).ToList();
            //if (id != -1)
            //{
            //    ViewBag.Product = _productRepository.Get(x => x.Id == id && x.IsActive);
            //}

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProImages vm)
        {
            //var product = ViewBag.Product;
            var currentUserId = GetCurrentUserId();

            foreach (var image in vm.Images)
            {
                string stringFileName = UploadFile(image);
                var productImage = new ProductImage
                {
                    Image = stringFileName,
                    ProductId = vm.Product.Id,
                    // Product = vm.Product,
                    CreatedById = currentUserId,
                };
                _productImageRepository.Add(productImage);
            }

            return RedirectToAction("List");
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
