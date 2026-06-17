using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers
{
            // [Authorize]

    [ApiController]
    [Route("api/shift")]
    public class ShiftController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public ShiftController(TaskDbContext context)
        {
            _context = context;
        }

        // 🔵 קבלת רשימת משמרות
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts(int skip = 0, int take = 7)
        {
            var shifts = await _context.Shifts
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(shifts);
        }

        // 🔵 קבלת משמרת לפי Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(Guid id)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
                return NotFound();

            return Ok(shift);
        }

        // 🔵 יצירת משמרת חדשה
        [HttpPost]
        public async Task<ActionResult<Shift>> CreateShift([FromBody] ShiftDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StartHour) ||
                string.IsNullOrWhiteSpace(dto.FinishHour) ||
                string.IsNullOrWhiteSpace(dto.Day))
            {
                return BadRequest("StartHour, FinishHour, and Day are required.");
            }

            var shift = new Shift
            {
                Id = Guid.NewGuid(),
                StartHour = dto.StartHour.Trim(),
                FinishHour = dto.FinishHour.Trim(),
                Day = dto.Day.Trim(),
                Specs = new(),
                AssignedEmployees = new(),
                PreferredEmployees = new()
            };

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shift);
        }

        // 🔵 עדכון משמרת
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(Guid id, [FromBody] ShiftDTO dto)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.StartHour))
                shift.StartHour = dto.StartHour.Trim();

            if (!string.IsNullOrWhiteSpace(dto.FinishHour))
                shift.FinishHour = dto.FinishHour.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Day))
                shift.Day = dto.Day.Trim();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔴 מחיקת משמרת
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(Guid id)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
                return NotFound();

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 בדיקה אם משמרת קיימת (optional)
        private bool ShiftExists(Guid id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}