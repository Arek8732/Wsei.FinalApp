using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly UniversityDbContext _db;
    public EnrollmentsController(UniversityDbContext db) => _db = db;

    public record EnrollCreate(int StudentId, int CourseId, int Grade);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrollment>>> Get() =>
        Ok(await _db.Enrollments.Include(e => e.Student).Include(e => e.Course).ToListAsync());

    [HttpPost]
    public async Task<ActionResult<Enrollment>> Post(EnrollCreate dto)
    {
        if (!await _db.Students.AnyAsync(s => s.Id == dto.StudentId))
            return BadRequest(new { Student = "The Student field is required." });
        if (!await _db.Courses.AnyAsync(c => c.Id == dto.CourseId))
            return BadRequest(new { Course = "The Course field is required." });

        var e = new Enrollment { StudentId = dto.StudentId, CourseId = dto.CourseId, Grade = dto.Grade };
        _db.Enrollments.Add(e); await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = e.Id }, e);
    }
}
