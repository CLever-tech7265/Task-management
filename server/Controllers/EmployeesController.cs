using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public EmployeesController(TaskDbContext context)
        {
            _context = context;
        }

        // מנהל רואה את כל העובדים
        [Authorize(Roles = "Manager")]
        [HttpGet("all-employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // רשימה חלקית
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(
            int skip = 0,
            int take = 3)
        {
            var result = await _context.Employees
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(result);
        }

        // חיפוש לפי שם
        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployee(
            string firstName,
            string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("First name and last name are required.");
            }

            var result = await _context.Employees
                .Where(e =>
                    e.FirstName.Contains(firstName.Trim()) &&
                    e.LastName.Contains(lastName.Trim()))
                .ToListAsync();

            return Ok(result);
        }

        // חיפוש לפי PeopleId
        [Authorize]
        [HttpGet("searchId")]
        public async Task<IActionResult> SearchEmployeeId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id is required");

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.PeopleId == id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // מחיקה
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.PeopleId == id);

            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // השלמת פרופיל
        [Authorize(Roles ="Employee")]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PeopleId = dto.PeopleId,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    UserId = userId,
                    Role = "Employee",
                    AssignedEmployees = new(),
                    PreferredEmployees = new()
                };

                _context.Employees.Add(employee);
            }
            else
            {
                employee.FirstName = dto.FirstName;
                employee.LastName = dto.LastName;
                employee.PeopleId = dto.PeopleId;
                employee.BirthDate = dto.BirthDate;
                employee.Email = dto.Email;
            }

            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        // 🔵 הוספת משמרת מועדפת לעובד
// 🔵 הוספת העדפת משמרת חדשה לעובד

[Authorize]
[HttpPost("create-with-preference")]
public async Task<IActionResult> CreateShiftWithPreference([FromBody] ShiftDTO dto)
{
    try
    {
        // 1. בדיקת DTO
        if (dto == null)
        {
            return BadRequest(new
            {
                message = "DTO is null"
            });
        }

        if (string.IsNullOrWhiteSpace(dto.StartHour) ||
            string.IsNullOrWhiteSpace(dto.FinishHour) ||
            string.IsNullOrWhiteSpace(dto.Day))
        {
            return BadRequest(new
            {
                message = "StartHour, FinishHour, Day are required"
            });
        }

        // 2. שליפת משתמש מהטוקן
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new
            {
                message = "Missing user claim"
            });
        }

        var userId = Guid.Parse(userIdClaim);

        // 3. שליפת עובד
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.UserId == userId);

        if (employee == null)
        {
            return NotFound(new
            {
                message = "Employee not found"
            });
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

        // 5. שמירה ראשונה כדי לוודא יצירת Shift ב-DB
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