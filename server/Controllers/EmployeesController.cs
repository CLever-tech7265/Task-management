using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
    // [Authorize]

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
        .Include(e => e.Specs)
        .FirstOrDefaultAsync(e => e.UserId == userId);

    var specs = await _context.Specialization
    .Where(s => dto.SpecializationIds.Contains(s.Id))
    .ToListAsync();

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
            Specs = new List<Specialization>()
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
        // ניקוי קשרים ישנים
        employee.Specs = specs;

    }

    // 🔥 שליפת ישויות אמיתיות מה-DB
    
    // 🔥 חיבור ל-navigation property
    await _context.SaveChangesAsync();

return Ok(new
{
    role = "Employee",
    profileCompleted = true
});
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