using GCD0805App.Models;
using GCD0805App.Units;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineesController : Controller
    {

        private ApplicationDbContext _context;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainee = _context.Users.SingleOrDefault(u => u.Id.Equals(userId));

            if (trainee == null) return HttpNotFound();
            return View(trainee);
        }

    }
}