using GCD0805App.Models;
using GCD0805App.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GCD0805App.Controllers
{

    public class CoursesController : Controller
    {
        public class CourseController : Controller
        {
            private ApplicationDbContext _context;
            private UserManager<ApplicationUser> _userManager;
            public CourseController()
            {
                _context = new ApplicationDbContext();
                _userManager = new UserManager<ApplicationUser>
                    (new UserStore<ApplicationUser>
                    (new ApplicationDbContext()));
            }

            public ActionResult Index(string searchString)
            {
                var courses = _context.Courses.ToList();

                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    courses = courses.Where(c => c.Name.ToLower().Contains(searchString)).ToList();
                }

                return View(courses);
            }

            public ActionResult Create()
            {
                var model = new GCD0805App.ViewModels.CoursesViewModel()
                {
                    Categories = _context.Categories.ToList()
                };
                return View(model);
            }

            [HttpPost]
            [Authorize(Roles = "Staff")]
            public ActionResult Create(CoursesViewModel model, int id)
            {
                if (ModelState.IsValid)
                {
                    var isNull = _context.Courses.SingleOrDefault(t => t.Name.Equals(model.Courses.Name));
                    if (isNull != null)
                    {
                        ViewBag.Error = "Name is already exist";
                        model.Categories = _context.Categories.ToList();
                        return View(model);
                    }
                    var newCourse = new Course
                    {
                        Name = model.Courses.Name,
                        Description = model.Courses.Description,
                        CategoryId = id
                    };
                    _context.Courses.Add(newCourse);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            [HttpGet]
            [Authorize(Roles = "Staff")]
            public ActionResult Details(int? id)
            {
                if (id == null) return HttpNotFound();

                var course = _context.Courses
                    .SingleOrDefault(t => t.Id == id);

                if (course == null) return HttpNotFound();

                return View(course);
            }

            public ActionResult Edit(int id)
            {
                var Course = _context.Courses.SingleOrDefault(t => t.Id == id);
                return View(Course);
            }

            [HttpPost]
            [Authorize(Roles = "Staff")]
            public ActionResult Edit(Course newCourse)
            {
                if (ModelState.IsValid)
                {
                    var Edit = _context.Courses.SingleOrDefault(t => t.Name == newCourse.Name);
                    if (Edit != null)
                    {
                        ViewBag.Error = "Name is already exist";
                        return View(newCourse);
                    }
                    var course = _context.Courses.SingleOrDefault(c => c.Id == newCourse.Id);
                    course.Name = newCourse.Name;
                    course.Description = newCourse.Description;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            public ActionResult Delete(int id)
            {
                var course = _context.Courses.SingleOrDefault(c => c.Id == id);
                _context.Courses.Remove(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            [HttpGet]
            public ActionResult ShowTrainers(int? id)
            {
                if (id == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                var members = _context.TrainingCourses
                    .Where(t => t.CourseId == id)
                    .Select(t => t.User);
                var trainer = new List<ApplicationUser>();

                foreach (var user in members)
                {
                    if (_userManager.GetRoles(user.Id)[0].Equals("Trainer"))
                    {
                        trainer.Add(user);
                    }
                }
                ViewBag.CourseId = id;
                return View(trainer);
            }

            [HttpGet]
            [Authorize(Roles = "Staff")]
            public ActionResult AddTrainers(int? id)
            {
                if (id == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                    return HttpNotFound();

                var usersInDb = _context.Users.ToList();

                var usersInTeam = _context.TrainingCourses
                    .Where(t => t.CourseId == id)
                    .Select(t => t.User)
                    .ToList();

                var usersToAdd = new List<ApplicationUser>();

                foreach (var user in usersInDb)
                {
                    if (!usersInTeam.Contains(user) &&
                        _userManager.GetRoles(user.Id)[0].Equals("Trainer"))
                    {
                        usersToAdd.Add(user);
                    }
                }

                var viewModel = new SharedCoursesViewModel
                {
                    CourseId = (int)id,
                    Users = usersToAdd
                };
                return View(viewModel);
            }

            [HttpPost]
            [Authorize(Roles = "Staff")]
            public ActionResult AddTrainers(TrainingCourse model)
            {
                var user = new TrainingCourse
                {
                    CourseId = model.CourseId,
                    UserId = model.UserId
                };

                _context.TrainingCourses.Add(user);
                _context.SaveChanges();

                return RedirectToAction("ShowTrainers", new { id = model.CourseId });
            }

            [HttpGet]
            [Authorize(Roles = "Staff")]
            public ActionResult RemoveTrainers(int id, string userId)
            {
                var courseUserToRemove = _context.TrainingCourses
                    .SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

                if (courseUserToRemove == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                _context.TrainingCourses.Remove(courseUserToRemove);
                _context.SaveChanges();
                return RedirectToAction("ShowTrainers", new { id = id });
            }

            [HttpGet]
            public ActionResult ShowTrainees(int? id)
            {
                if (id == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                var members = _context.TrainingCourses
                    //.Include(t => t.User)
                    .Where(t => t.CourseId == id)
                    .Select(t => t.User);
                var trainee = new List<ApplicationUser>();       // Init List Users to Add Course

                foreach (var user in members)
                {
                    if (_userManager.GetRoles(user.Id)[0].Equals("Trainee"))
                    {
                        trainee.Add(user);
                    }
                }
                ViewBag.CourseId = id;
                return View(trainee);
            }

            [HttpGet]
            [Authorize(Roles = "Staff")]
            public ActionResult AddTrainees(int? id)
            {
                if (id == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                    return HttpNotFound();

                var usersInDb = _context.Users.ToList();

                var usersInTeam = _context.TrainingCourses

                    .Where(t => t.CourseId == id)
                    .Select(t => t.User)
                    .ToList();

                var usersToAdd = new List<ApplicationUser>();

                foreach (var user in usersInDb)
                {
                    if (!usersInTeam.Contains(user) &&
                        _userManager.GetRoles(user.Id)[0].Equals("Trainee"))
                    {
                        usersToAdd.Add(user);
                    }
                }

                var viewModel = new SharedCoursesViewModel
                {
                    CourseId = (int)id,
                    Users = usersToAdd
                };
                return View(viewModel);
            }

            [HttpPost]
            [Authorize(Roles = "Staff")]
            public ActionResult AddTrainees(TrainingCourse model)
            {
                var courseUser = new TrainingCourse
                {
                    CourseId = model.CourseId,
                    UserId = model.UserId
                };

                _context.TrainingCourses.Add(courseUser);
                _context.SaveChanges();

                return RedirectToAction("ShowTrainees", new { id = model.CourseId });
            }
            [HttpGet]
            [Authorize(Roles = "Staff")]
            public ActionResult RemoveTrainees(int id, string userId)
            {
                var courseUserToRemove = _context.TrainingCourses
                    .SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

                if (courseUserToRemove == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                _context.TrainingCourses.Remove(courseUserToRemove);
                _context.SaveChanges();

                return RedirectToAction("ShowTrainees", new { id = id });
            }

            [HttpGet]
            [Authorize(Roles = "Trainer, Trainee")]
            public ActionResult MyCourse()
            {
                var userId = User.Identity.GetUserId();

                var courses = _context.TrainingCourses
                    .Where(t => t.UserId.Equals(userId))
                    .Select(t => t.Course)
                    .ToList();

                return View(courses);
            }
            public ActionResult ListUser(string searchCourse, string role)
            {
                var getRole = _context.Roles.SingleOrDefault(r => r.Name == role);
                var Course = _context.Courses.Where(m => m.Name.Contains(searchCourse));
                var lstUser = _context.TrainingCourses
                .Include("User")
                .Include("Course")
                .Where(m => Course.Any(x => x.Id == m.Course.Id)).ToList();
                var lstUserByRole = lstUser.Where(m => m.User.Roles.Any(x => x.RoleId == getRole.Id)).ToList();
                return View(lstUserByRole);
            }
        }
    }
}