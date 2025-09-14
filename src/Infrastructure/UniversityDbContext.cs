using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) {}

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Student>().HasIndex(s => s.Email).IsUnique();

        b.Entity<Enrollment>()
            .HasOne(e => e.Student).WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId).IsRequired();

        b.Entity<Enrollment>()
            .HasOne(e => e.Course).WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId).IsRequired();

        // Seed
        b.Entity<Student>().HasData(
            new Student { Id = 1, Email = "john@uni.edu", FirstName = "John", LastName = "Doe" },
            new Student { Id = 2, Email = "eva@uni.edu",  FirstName = "Eva",  LastName = "Nowak" }
        );
        b.Entity<Course>().HasData(
            new Course { Id = 1, Title = "Distributed Systems", Credits = 6 },
            new Course { Id = 2, Title = "Databases", Credits = 5 }
        );
        b.Entity<Enrollment>().HasData(
            new Enrollment { Id = 1, StudentId = 1, CourseId = 1, Grade = 5 }
        );
    }
}
