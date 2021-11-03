using GCD0805App.Models;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class GroupsViewModel
    {
        public List<IdentityModel> Staffs { get; set; }
        public List<IdentityModel> Trainers { get; set; }
        public List<IdentityModel> Trainees { get; set; }
    }
}