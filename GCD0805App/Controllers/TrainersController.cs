using GCD0805App.Models;
using GCD0805App.Units;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainerController : Controller
    {
        private ApplicationDbContext _context;
        public TrainerController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainer = _context.Users.SingleOrDefault(t => t.Id.Equals(userId));

            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        [HttpGet]
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
        public ActionResult Edit(UserRolesViewModel model)
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
    }
}