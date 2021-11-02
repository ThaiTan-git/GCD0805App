using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GCD0805App.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<TrainerCourse> TrainerCourses { get; set; }
        public DbSet<TraineeCourse> TraineeCourses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}