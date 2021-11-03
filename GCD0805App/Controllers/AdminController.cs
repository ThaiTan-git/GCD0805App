using GCD0805App.Models;
using GCD0805App.Units;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = Role.Admin)]
        public ActionResult Index()
        {
            var trainerRole = _context.Roles.SingleOrDefault(r => r.Name == Role.Trainer);
            var staffRole = _context.Roles.SingleOrDefault(r => r.Name == Role.Staff);

            var model = new GroupsViewModel()
            {
                Trainers = _context.Users
                    .Where(u => u.Roles.Any(r => r.RoleId == trainerRole.Id))
                    .ToList(),
                Staffs = _context.Users
                    .Where(u => u.Roles.Any(r => r.RoleId == staffRole.Id))
                    .ToList(),
            };
            return View(model);
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManger
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new RegisterViewModel
            {
                Roles = new List<string>() { Role.Trainer, Role.Staff }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address
                };

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(t => t.Id.Equals(userId));

            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UserRolesViewModel()
            {
                Users = user,
                Roles = new List<string>(_userManager.GetRoles(User.Identity.GetUserId()))
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.Users;
                var userId = User.Identity.GetUserId();
                var userInDb = _context.Users.SingleOrDefault(t => t.Id.Equals(userId));

                if (userInDb == null)
                    return HttpNotFound();
                userInDb.Name = user.Name;
                userInDb.Age = user.Age;
                userInDb.DateOfBirth = user.DateOfBirth;
                userInDb.Email = user.Email;
                userInDb.UserName = user.Email;


                IdentityResult result = UserManager.Update(userInDb);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var user = UserManager.FindById(id);
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ResetPassword(string email)
        {
            TempData["userEmail"] = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var email = (string)TempData["userEmail"];
            var user = UserManager.FindByEmail(model.Email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "The user does not exist";
                return View(model);
            }

            var roles = UserManager.GetRoles(User.Identity.GetUserId());
            if (!roles.All(r => r == Role.Staff || r == Role.Trainer))
            {
                ViewBag.ErrorMessage = "The user cannot be reset. Permission is denied.";
                return View(model);
            }

            model.Code = UserManager.GeneratePasswordResetToken(User.Identity.GetUserId());
            IdentityResult result = UserManager.ResetPassword(User.Identity.GetUserId(), model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            AddErrors(result);
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}