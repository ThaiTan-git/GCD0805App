using GCD0805App.Models;
using GCD0805App.Units;
using System.Collections.Generic;

namespace GCD0805App.ViewModels
{
    public class UserRolesViewModel
    {
        public ApplicationUser Users { get; set; }
        public List<string> Roles { get; set; }
    }
}