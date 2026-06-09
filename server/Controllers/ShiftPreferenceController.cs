using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers
{
    [Route("api/shift-preferences")]
    [ApiController]
    [Authorize]
    public class ShiftPreferenceController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public ShiftPreferenceController(TaskDbContext context)
        {
            _context = context;
        }
        [HttpPost("create-with-preference")]
        public async Task<IActionResult> CreateShiftWithPreference([FromBody] ShiftDTO dto)
        {
            try
            {
                // 1. בדיקת DTO
                if (dto == null)
                {
                    return BadRequest(new { message = "DTO is null" });
                }

                if (string.IsNullOrWhiteSpace(dto.StartHour) ||
                    string.IsNullOrWhiteSpace(dto.FinishHour) ||
                    string.IsNullOrWhiteSpace(dto.Day))
                {
                    return BadRequest(new { message = "StartHour, FinishHour, Day are required" });
                }

                // 2. שליפת משתמש מהטוקן
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { message = "Missing user claim" });
                }

                var userId = Guid.Parse(userIdClaim);

                // 3. שליפת עובד
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == userId);

                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                // 4. יצירת Shift
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

                await _context.Shifts.AddAsync(shift);

                // 5. שמירה ראשונה
                await _context.SaveChangesAsync();

                // 6. יצירת Preference
                var preference = new EmployeeShiftPreference
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = employee.Id,
                    ShiftId = shift.Id
                };

                await _context.EmployeeShiftPreferences.AddAsync(preference);

                // 7. שמירה שנייה
                await _context.SaveChangesAsync();

                // 8. החזרה
                return Ok(new CreateShiftResponseDto
                {
                    ShiftId = shift.Id,
                    StartHour = shift.StartHour,
                    FinishHour = shift.FinishHour,
                    Day = shift.Day,
                    PreferenceId = preference.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [HttpGet("my-shifts")]
public async Task<IActionResult> GetMyShifts()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdClaim))
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.UserId == userId);

    if (employee == null)
        return NotFound("Employee not found");

    var shifts = await _context.EmployeeShiftPreferences
        .Where(p => p.EmployeeId == employee.Id)
        .Include(p => p.Shift)
        .Select(p => new
        {
            PreferenceId = p.Id,
            ShiftId = p.Shift.Id,
            p.Shift.StartHour,
            p.Shift.FinishHour,
            p.Shift.Day
        })
        .ToListAsync();

    return Ok(shifts);
}
[HttpPut("{preferenceId}")]
public async Task<IActionResult> UpdateShift(Guid preferenceId, [FromBody] ShiftDTO dto)
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdClaim))
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.UserId == userId);

    if (employee == null)
        return NotFound("Employee not found");

    var preference = await _context.EmployeeShiftPreferences
        .Include(p => p.Shift)
        .FirstOrDefaultAsync(p => p.Id == preferenceId && p.EmployeeId == employee.Id);

    if (preference == null)
        return NotFound("Preference not found");

    // עדכון ה-Shift עצמו
    preference.Shift.StartHour = dto.StartHour;
    preference.Shift.FinishHour = dto.FinishHour;
    preference.Shift.Day = dto.Day;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        preference.Id,
        preference.Shift.StartHour,
        preference.Shift.FinishHour,
        preference.Shift.Day
    });
}
[HttpDelete("{preferenceId}")]
public async Task<IActionResult> DeleteShift(Guid preferenceId)
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdClaim))
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.UserId == userId);

    if (employee == null)
        return NotFound("Employee not found");

    var preference = await _context.EmployeeShiftPreferences
        .Include(p => p.Shift)
        .FirstOrDefaultAsync(p => p.Id == preferenceId && p.EmployeeId == employee.Id);

    if (preference == null)
        return NotFound("Preference not found");

    _context.EmployeeShiftPreferences.Remove(preference);

    // אופציונלי: למחוק גם את ה-Shift עצמו
    _context.Shifts.Remove(preference.Shift);

    await _context.SaveChangesAsync();

    return NoContent();
}
            public class CreateShiftResponseDto
    {
        public Guid ShiftId { get; set; }
        public string StartHour { get; set; }
        public string FinishHour { get; set; }
        public string Day { get; set; }
        public Guid PreferenceId { get; set; }
    }
        }
}