namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int Credits { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
