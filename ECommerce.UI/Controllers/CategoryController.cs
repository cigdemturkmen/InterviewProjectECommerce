using ECommerce.Data.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public IActionResult List()
        {
            var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x =>
            new CategoryViewModel()
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
            }).ToList();

            return View(categories);
        }

        [AllowAnonymous]
        public PartialViewResult _SidebarPartialView()
        {
            var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x =>
            new CategoryViewModel()
            {
                Id = x.Id,
                CategoryName = x.CategoryName,

            }).ToList();

            return PartialView(categories);
        }

        public IActionResult Detail(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id && x.IsActive, x => x.Include(y => y.Products));

            var vm = new CategoryViewModel()
            {
                Id = id,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
                Products = category.Products,
            };
            return RedirectToAction("List", "Product");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = GetCurrentUserId();

            Category category = new Category()
            {
                CategoryName = model.CategoryName,
                CreatedById = currentUserId,
                CreatedDate = DateTime.Now,
            };

            var result = _categoryRepository.Add(category);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Add", model);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id && x.IsActive);

            if (category != null)
            {
                var vm = new CategoryViewModel()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    IsActive = category.IsActive,
                };

                return View(vm);
            }

            TempData["Message"] = "Category cannot be found!";
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var currentUserId = GetCurrentUserId();

            Category entity = new Category()
            {
                Id = model.Id,
                CategoryName = model.CategoryName,
                IsActive = model.IsActive,
                UpdatedById = currentUserId,
                UpdatedDate = DateTime.Now,
            };

            var result = _categoryRepository.Edit(entity);

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
            var category = _categoryRepository.Get(x => x.Id == id && x.IsActive);
            category.UpdatedDate = DateTime.Now;
            category.UpdatedById = currentUserId;

            var result = _categoryRepository.Delete(id);

            TempData["Message"] = result ? "Deleted" : "Delete failed";

            return RedirectToAction("List");
        }
    }
}
