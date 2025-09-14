namespace Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
