using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers
{
    // [Authorize]

    [ApiController]
    [Route("api/specialization")]
    public class SpecializationController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public SpecializationController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations(
            int skip = 0,
            int take = 10)
        {
            return await _context.Specialization
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(Guid id)
        {
            var specialization = await _context.Specialization
                .Include(s => s.Employees)
                .Include(s => s.Shifts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        [HttpPost]
        public async Task<ActionResult<Specialization>> CreateSpecialization(
            SpecializationDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Name is required.");
            }

            if (await _context.Specialization.AnyAsync(
                    s => s.Name == dto.Name.Trim()))
            {
                return Conflict("Specialization already exists.");
            }

            var specialization = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Discription="",
                Employees = [],
                Shifts = []
            };

            _context.Specialization.Add(specialization);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSpecialization),
                new { id = specialization.Id },
                specialization);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(
            Guid id,
            SpecializationDTO dto)
        {
            var specialization =
                await _context.Specialization.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                specialization.Name = dto.Name.Trim();
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(Guid id)
        {
            var specialization =
                await _context.Specialization.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            _context.Specialization.Remove(specialization);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}