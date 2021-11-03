using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCD0805App.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Education { get; set; }
        [Required]
        public string Specialty { get; set; }

    }
}