using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GCD0805App.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [StringLength(225)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Education { get; set; }

        [StringLength(255)]
        public string Specialty { get; set; }
    }
}
