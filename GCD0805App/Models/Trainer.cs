using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCD0805App.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Specialty { get; set; }

        [ForeignKey("User")]
        public String TrainerId { get; set; }
        public ApplicationUser User { get; set; }
    }
}