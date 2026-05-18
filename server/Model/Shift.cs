using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Shift{
      
        public Guid Id { get; set; }
        public string StartHour { get; set; }="";
        public string FinishHour { get; set; }="";
        public List<Specialization> Specs { get; set; }=[];
    }
    public class ShiftDTO
    {
        public Guid? Id{get;set;}
        public string? StartHour { get; set; }
       public string? FinishHour { get; set; }

    }
}