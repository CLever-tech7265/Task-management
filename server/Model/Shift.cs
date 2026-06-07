using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Shift{
      
        public Guid Id { get; set; }
        public string StartHour { get; set; }="";
        public string FinishHour { get; set; }="";
        public string Day { get; set; }
        public List<Specialization> Specs { get; set; }=[];
    public List<EmployeeAssignedShift> AssignedEmployees { get; set; } = new();
    public List<EmployeeShiftPreference> PreferredEmployees { get; set; } = new();
    
    }
    public class ShiftDTO
    {
       public string? StartHour { get; set; }
       public string? FinishHour { get; set; }
       public string? Day { get; set; }


    }
}