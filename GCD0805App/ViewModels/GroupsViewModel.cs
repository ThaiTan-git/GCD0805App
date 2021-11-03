using GCD0805App.Models;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class GroupsViewModel
    {
        public List<User> Staffs { get; set; }
        public List<User> Trainers { get; set; }
        public List<User> Trainees { get; set; }
    }
}