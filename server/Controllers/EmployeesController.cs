using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController:ControllerBase{
        private readonly TaskDbContext _context;

        public EmployeesController(TaskDbContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int skip=0,int take=3)
        {
             return await _context.Employees.Skip(skip).Take(take).ToListAsync();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployee(
            string? firsName,
            string? lastName
        )
        {
              if (string.IsNullOrWhiteSpace(firsName)||string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("First name and last name query parameter is required.");
            }

            return await _context
                 .Employees.Where(a => 
                a.FirstName != null && a.FirstName.Contains(firsName.Trim())
               && a.LastName != null && a.LastName.Contains(lastName.Trim()))
                
                .ToListAsync();
        }

        
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Employee>>> CreateEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FirstName)||string.IsNullOrWhiteSpace(employee.LastName))
            {
                return BadRequest("Empoyee name is required.");
            }
            if (
                _context.Employees.Any(a =>
                    a.PeopleId != null && a.PeopleId == employee.PeopleId
                )
            )
            {
                return Conflict("An employee with the same id already exists.");
            }
            
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        }

    }
}