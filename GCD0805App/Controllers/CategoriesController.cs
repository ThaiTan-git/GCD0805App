using GCD0805App.Models;
using GCD0805App.Units;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Categories
        public ActionResult Index(string searchString)
        {
            var categories = _context.Categories.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                categories = categories.Where(c => c.Name.ToLower().Contains(searchString)).ToList();
            }

            return View(categories);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            var Category = _context.Categories.SingleOrDefault(t => t.Name == category.Name);
            if (Category != null)
            {
                ViewBag.Error = "Name is already exist";
                return View(category);
            }

            var newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                var Edit = _context.Categories.SingleOrDefault(t => t.Name == newCategory.Name);
                if (Edit != null)
                {
                    ViewBag.Error = "Name is already exist";
                    return View(newCategory);
                }
                var oldCategory = _context.Categories.SingleOrDefault(c => c.Id == newCategory.Id);
                oldCategory.Name = newCategory.Name;
                oldCategory.Description = newCategory.Description;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}