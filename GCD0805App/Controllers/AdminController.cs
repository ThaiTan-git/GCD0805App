using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = Role.Admin)]
        // GET: Admin/Account
        public ActionResult Index()
        {
            var trainerRole = _context.Roles.SingleOrDefault(r => r.Name == Role.Trainer);
            var staffRole = _context.Roles.SingleOrDefault(r => r.Name == Role.Staff);

            var model = new UsersGroupViewModel()
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
            var model = new ViewModels.AccountViewModel()
            {
                Roles = new List<string>() { Role.Trainer, Role.Staff }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Age = model.Age,
                    Address = model.Address
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new InfoViewModel()
            {
                User = user,
                Roles = new List<string>(await UserManager.GetRolesAsync(id))
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(InfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.User;
                var userinDb = UserManager.FindById(user.Id);

                if (userinDb == null)
                    return HttpNotFound();
                userinDb.FullName = user.FullName;
                userinDb.Age = user.Age;
                userinDb.DateofBirth = user.DateofBirth;
                userinDb.Email = user.Email;
                userinDb.UserName = user.Email;


                IdentityResult result = UserManager.Update(userinDb);

                // if (model.Specialty != null)
                // {
                //    var profile = await _context.TrainerProfiles.SingleOrDefaultAsync(p => p.UserId == userinDb.Id);
                //    profile.Specialty = model.Specialty;
                // }
                _context.SaveChanges();

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
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

            var roles = UserManager.GetRoles(user.Id);
            if (!roles.All(r => r == Role.Staff || r == Role.Trainer))
            {
                ViewBag.ErrorMessage = "The user cannot be reset. Permission is denied.";
                return View(model);
            }

            model.Code = UserManager.GeneratePasswordResetToken(user.Id);
            IdentityResult result = UserManager.ResetPassword(user.Id, model.Code, model.Password);

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