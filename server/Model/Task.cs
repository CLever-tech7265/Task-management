using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.modules
{
    public class Task
    {
        public Guid Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Shift")]
        public Shift Shifts { get; set; }
        [ForeignKey("Specialization")]
        public Specialization Specializations { get; set; }
    }
    public class TaskDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}