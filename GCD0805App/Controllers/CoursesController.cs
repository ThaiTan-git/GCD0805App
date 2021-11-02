using GCD0805App.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{

    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Courses
        [HttpGet]
        public ActionResult Index(string searchCourse)
        {
            var courses = _context.Courses
                .Include(t => t.Category)
                .ToList();

            if (!string.IsNullOrEmpty(searchCourse))
            {
                courses = courses
                    .Where(t => t.Name.ToLower().Contains(searchCourse.ToLower())).
                    ToList();
            }
            return View(courses);
        }
    }
}