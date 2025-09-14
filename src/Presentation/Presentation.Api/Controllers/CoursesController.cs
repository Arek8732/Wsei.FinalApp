using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly UniversityDbContext _db;
    public CoursesController(UniversityDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> Get() =>
        Ok(await _db.Courses.ToListAsync());

    [HttpPost]
    public async Task<ActionResult<Course>> Post(Course c)
    {
        _db.Courses.Add(c); await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }
}
