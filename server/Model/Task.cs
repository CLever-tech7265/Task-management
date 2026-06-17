using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.modules
{
  public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    public ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
public class CreateTaskRequest
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Guid> ShiftIds { get; set; } = new();
    public List<Guid> SpecializationIds { get; set; } = new();
}
    public class TaskDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}