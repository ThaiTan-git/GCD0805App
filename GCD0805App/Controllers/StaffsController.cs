using GCD0805App.Models;
using GCD0805App.Units;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class StaffController : Controller
    {

        private readonly ApplicationDbContext _context;

        public StaffController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string searchString)
        {
            var role = _context.Roles.SingleOrDefault(r => r.Name == Role.Trainee);
            var data = new GroupsViewModel
            {
                Trainees = _context.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToList()
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data.Trainees = data.Trainees.Where((c => !String.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(searchString)
                || searchString.Contains(c.Age.ToString()))).ToList();
                return View(data);

            }
            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    Age = model.Age,
                    DateOfBirth = model.DateOfBirth,
                    Education = model.Education
                };
                _context.Users.Add(user);
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var userInDb = _context.Users.SingleOrDefault(u => u.Id.Equals(userId));
            if (userInDb == null)
            {
                return HttpNotFound();
            };
            var viewModel = new UserRolesViewModel
            {
                Users = userInDb
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.Users;
                var userInDb = _context.Users.SingleOrDefault(u => u.Id.Equals(user));

                if (userInDb == null)
                {

                    return HttpNotFound();

                }

                userInDb.UserName = user.UserName;
                userInDb.Email = user.Email;
                userInDb.Age = user.Age;
                userInDb.DateOfBirth = user.DateOfBirth;
                userInDb.Address = user.Address;
                userInDb.Specialty = user.Specialty;

                return RedirectToAction("Index");
            }
            return View(model);
        }



        public ActionResult Delete(string id)
        {
            var todoInDb = _context.Users
                .SingleOrDefault(t => t.Equals(id));
            if (todoInDb == null)
            {
                return HttpNotFound();
            };
            _context.Users.Remove(todoInDb);
            return RedirectToAction("Index");
        }
    }
}