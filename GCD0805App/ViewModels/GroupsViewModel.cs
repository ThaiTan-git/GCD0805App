using GCD0805App.Models;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class GroupsViewModel
    {
        public List<ApplicationUser> Staffs { get; set; }
        public List<ApplicationUser> Trainers { get; set; }
        public List<ApplicationUser> Trainees { get; set; }
    }
}