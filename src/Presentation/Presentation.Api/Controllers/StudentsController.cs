using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly UniversityDbContext _db;
    public StudentsController(UniversityDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Get() =>
        Ok(await _db.Students.Include(s => s.Enrollments).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Student>> Get(int id)
    {
        var s = await _db.Students.Include(s => s.Enrollments).FirstOrDefaultAsync(s => s.Id == id);
        return s is null ? NotFound() : Ok(s);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Post(Student s)
    {
        _db.Students.Add(s);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = s.Id }, s);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Student>> Put(int id, Student s)
    {
        var existing = await _db.Students.FindAsync(id);
        if (existing is null) return NotFound();
        existing.FirstName = s.FirstName; existing.LastName = s.LastName; existing.Email = s.Email;
        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Students.FindAsync(id);
        if (s is null) return NotFound();
        _db.Remove(s); await _db.SaveChangesAsync();
        return NoContent();
    }
}
