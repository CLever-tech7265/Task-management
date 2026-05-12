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
        // [httpPost]
        // public async Task<ActionResult<Artist>> CreateEmployee(ArtistDTO artist)
    }
}