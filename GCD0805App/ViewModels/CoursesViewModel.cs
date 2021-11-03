using GCD0805App.Models;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class CoursesViewModel
    {
        public Course Courses { get; set; }
        public List<Category> Categories { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}