using ECommerce.Data.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Controllers
{
    [Authorize(Roles = "1")]
    public class ProductController : BaseController
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductController(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public IActionResult Index(int? id)
        {
            var products = _productRepository.GetAll(x => x.IsActive, include: x => x.Include(y => y.Category).Include(y => y.ProductImages));

            var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x =>
            new CategoryViewModel()
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
            }).ToList();

            if (id != null)
            {
                products = _productRepository.GetAll(x => x.IsActive && x.CategoryId == id, x => x.Include(y => y.ProductImages).Include(y => y.Category));
            }

            var vm = products.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                CreatedDate = x.CreatedDate,
                CategoryName = x.Category.CategoryName,
                ProductImages = x.ProductImages,
            }).ToList();

            ViewBag.Categories = categories;
            return View(vm);
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepository.Get(x => x.Id == id && x.IsActive, include: x => x.Include(y => y.Category).Include(y => y.ProductImages));

            if (product != null)
            {
                var vm = new ProductViewModel()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    ProductImages = product.ProductImages,
                };
                return View(vm);
            }

            TempData["Message"] = "Product cannot be found.";
            return RedirectToAction("Index");
        }

        public IActionResult List(int? id)
        {
            var products = _productRepository.GetAll(x => x.IsActive, include: x => x.Include(y => y.Category));

            if (id != null)
            {
                products = _productRepository.GetAll(x => x.CategoryId == id);
            }
            
            var vm = products.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                ProductImages = x.ProductImages,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
            }).ToList();

            return View(vm);
        }

        public IActionResult Add()
        {
            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();
                return View(model);
            }

            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();

            var currentUserId = GetCurrentUserId();

            Product product = new Product()
            {
                ProductName = model.ProductName,
                UnitPrice = model.UnitPrice,
                ProductImages = model.ProductImages,
                CreatedDate = DateTime.Now,
                CreatedById = currentUserId,
                CategoryId = model.CategoryId,
            };

            var result = _productRepository.Add(product);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Add", model);
        }


        public IActionResult Edit(int id)
        {
            var product = _productRepository.Get(x => x.Id == id && x.IsActive, include: x => x.Include(y => y.Category));

            if (product != null)
            {
                var vm = new ProductViewModel()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    Category = product.Category,
                    IsActive = product.IsActive,
                    UnitPrice = product.UnitPrice,
                    ProductImages = product.ProductImages,
                };

                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();

                return View(vm);
            }

            TempData["Message"] = "Product cannot be found!";
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();

                return View("Edit", model);
            }

            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();

            var currentUserId = GetCurrentUserId();

            Product product = new Product()
            {
                Id = model.Id,
                ProductName = model.ProductName,
                IsActive = model.IsActive,
                UpdatedById = currentUserId,
                UpdatedDate = DateTime.Now,
                UnitPrice = model.UnitPrice,
                ProductImages = model.ProductImages,
                CategoryId = model.CategoryId,
            };

            var result = _productRepository.Edit(product);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Edit", model);
        }


        public IActionResult Delete(int id)
        {
            var currentUserId = GetCurrentUserId();
            var product = _productRepository.Get(x => x.Id == id && x.IsActive);
            product.UpdatedDate = DateTime.Now;
            product.UpdatedById = currentUserId;

            var result = _productRepository.Delete(id);

            TempData["Message"] = result ? "Deleted" : "Delete failed";

            return RedirectToAction("List");
        }
    }
}
