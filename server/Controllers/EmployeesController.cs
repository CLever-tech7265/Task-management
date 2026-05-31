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
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> CreateEmployee(EmployeeDto employeeDto)
        {


//             if(employeeDto.Specs != null && employeeDto.Specs.Any())
// {
//     employee.Specs = employeeDto.Specs.Select(s => new Specialization { Id = s.Id, Name = s.Name }).ToList();
// }
// else
// {
//     employee.Specs = new List<Specialization>(); // או השאר null אם השתמשת ב־List<Specialization>?
// }
            if (string.IsNullOrWhiteSpace(employeeDto.FirstName)||string.IsNullOrWhiteSpace(employeeDto.LastName))
            {
                return BadRequest("Employee name is required miri.");
            }
            if (
                await _context.Employees.AnyAsync(a =>
                    a.PeopleId != null && a.PeopleId == employeeDto.PeopleId
                )
            )
            {
                return Conflict("An employee with the same id already exists.");
            }
            var level=new Level{
                    Id=Guid.NewGuid(),
                    EmployeeLevel="Normal"
            };

         

             var employee=new Employee
            {
                Id=Guid.NewGuid(),
                FirstName = employeeDto.FirstName.Trim(),
                LastName = employeeDto.LastName.Trim(),
                PeopleId = employeeDto.PeopleId.Trim(),
                BirthDate = employeeDto.BirthDate,
                Email = employeeDto.Email.Trim(),
                Specs = new List<Specialization> {},
                
            
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(SearchEmployeeId), employee);
        }



        // [HttpPut("{id}")]
        // public async Task<ActionResult<EmployeeDto>> UpdateEmployee(string id, EmployeeDto emp)
        // {
        //     if (id != emp.PeopleId)
        //     {
        //         return BadRequest();
        //     }

        //     if (!EmployeeExists(id))
        //     {
        //         return NotFound();
        //     }

        //     var employeeEntity = await _context.Employees.FindAsync(id);
        //     employeeEntity.PeopleId = emp.PeopleId;

        //     _context.Entry(employeeEntity).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         throw;
        //     }
        //     return NoContent();
        // }


[HttpPut("{id}")]
public async Task<IActionResult> UpdateEmployee(string id, EmployeeDto emp)
{
    // if (!Guid.TryParse(id, out var guidId))
    //     return BadRequest("Invalid employee id.");

    var employee = await _context.Employees
        .Include(e => e.Specs)
        .Include(e => e.Level)
        .FirstOrDefaultAsync(e => e.PeopleId == id);

    if (employee == null)
        return NotFound();

    // עדכון שדות בסיסיים
    employee.FirstName = emp.FirstName?.Trim();
    employee.LastName = emp.LastName?.Trim();
    employee.PeopleId = emp.PeopleId?.Trim();
    employee.Email = emp.Email?.Trim();
    employee.BirthDate = emp.BirthDate;

    // עדכון Level (אם נשלח חדש)
    // if (emp.LevelId != Guid.Empty)
    // {
    //     var levelExists = await _context.EmployeeLevels
    //         .AnyAsync(l => l.Id == emp.LevelId);

    //     if (!levelExists)
    //         return BadRequest("Level not found.");

    //     employee.LevelId = emp.LevelId;
    // }

    // עדכון Specs (אם נשלח)
    // if (emp.Specs != null)
    // {
    //     employee.Specs.Clear();

    //     var specs = await _context.Specializations
    //         .Where(s => emp.Specs.Select(x => x.Id).Contains(s.Id))
    //         .ToListAsync();

    //     employee.Specs = specs;
    // }

    await _context.SaveChangesAsync();

    return Ok(employee);
}


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.PeopleId == id);
            if (emp == null)
            {
                return NotFound();
            }

           _context.Employees.Remove(emp);
            await _context.SaveChangesAsync(); 
           
            return Ok(emp);
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.PeopleId == id);
        }
 
}

}