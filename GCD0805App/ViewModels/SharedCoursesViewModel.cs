using GCD0805App.Models;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class SharedCoursesViewModel
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<IdentityModel> Users { get; set; }
    }
}