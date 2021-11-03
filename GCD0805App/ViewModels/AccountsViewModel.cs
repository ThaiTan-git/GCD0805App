using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCD0805App.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "FullName")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "DateTime")]
        public DateTime DateofBirth { get; set; }


        [Display(Name = "Education")]
        public string Education { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; }

        public List<string> Roles { get; set; }

    }
}