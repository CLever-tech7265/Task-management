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


      [HttpGet("searchId")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployeeId(
            string id
        )
        {
              if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id query parameter is required.");
            }

            return await _context
                 .Employees.Where(a => 
                a.PeopleId != null && a.PeopleId==id).ToListAsync();
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

            return CreatedAtAction(nameof(SearchEmployeeId), new { id = employee.PeopleId }, employee);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(string id, Employee emp)
        {
            if (id != emp.PeopleId)
            {
                return BadRequest();
            }

            if (!EmployeeExists(id))
            {
                return NotFound();
            }

            var employeeEntity = await _context.Employees.FindAsync(id);
            employeeEntity.PeopleId = emp.PeopleId;

            _context.Entry(employeeEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

           _context.Employees.Remove(emp);
            await _context.SaveChangesAsync(); 
           
            return NoContent();
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.PeopleId == id);
        }
 
}

}