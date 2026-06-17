using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers
{
// [Authorize(Roles = "Manager")]
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
public async Task<ActionResult<TaskManagement.modules.Task>> CreateTask(CreateTaskRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return BadRequest("Name is required");

    var shifts = await _context.Shifts
        .Where(s => request.ShiftIds.Contains(s.Id))
        .ToListAsync();

    var specs = await _context.Specialization
        .Where(s => request.SpecializationIds.Contains(s.Id))
        .ToListAsync();

    var task = new TaskManagement.modules.Task
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        Description = request.Description?.Trim(),
        Shifts = shifts,
        Specializations = specs
    };

    _context.Tasks.Add(task);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
}

        [HttpPut("{id}")]
public async Task<IActionResult> UpdateTask(Guid id, CreateTaskRequest request)
{
    var task = await _context.Tasks
        .Include(t => t.Shifts)
        .Include(t => t.Specializations)
        .FirstOrDefaultAsync(t => t.Id == id);

    if (task == null)
        return NotFound();

    task.Name = request.Name;
    task.Description = request.Description;

    task.Shifts = await _context.Shifts
        .Where(s => request.ShiftIds.Contains(s.Id))
        .ToListAsync();

    task.Specializations = await _context.Specialization
        .Where(s => request.SpecializationIds.Contains(s.Id))
        .ToListAsync();

    await _context.SaveChangesAsync();

    return NoContent();
}

        [HttpDelete("{id}")]
public async Task<IActionResult> DeleteTask(Guid id)
{
    var task = await _context.Tasks
        .Include(t => t.Shifts)
        .Include(t => t.Specializations)
        .FirstOrDefaultAsync(t => t.Id == id);

    if (task == null)
        return NotFound();

    task.Shifts.Clear();
    task.Specializations.Clear();

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