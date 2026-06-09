using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers
{
[Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TaskController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskManagement.modules.Task>>> GetTasks(
            int skip = 0,
            int take = 10)
        {
            return await _context.Tasks
                .Include(t => t.Shifts)
                .Include(t => t.Specializations)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskManagement.modules.Task>> GetTask(Guid id)
        {
            var task = await _context.Tasks
                .Include(t => t.Shifts)
                .Include(t => t.Specializations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpPost]
        public async Task<ActionResult<TaskManagement.modules.Task>> CreateTask(TaskDto taskDto)
        {
            try{
            if (string.IsNullOrWhiteSpace(taskDto.Name))
            {
                return BadRequest("Name is required");
            }

            var task = new TaskManagement.modules.Task
            {
                Id = Guid.NewGuid(),
                Name = taskDto.Name.Trim(),
                Description = taskDto.Description?.Trim()
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                task);
        }
        catch (Exception ex)
{
    return StatusCode(500, ex.Message);
}
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(
            Guid id,
            TaskDto taskDto)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(taskDto.Name))
            {
                task.Name = taskDto.Name.Trim();
            }

            if (!string.IsNullOrWhiteSpace(taskDto.Description))
            {
                task.Description = taskDto.Description.Trim();
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(t => t.Id == id);
        }
    }
}