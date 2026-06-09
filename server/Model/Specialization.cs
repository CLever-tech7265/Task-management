using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Specialization{
        
        public Guid Id { get; set; }
        public string Name { get; set; }="";
        public string Discription {get;set;}="";
        public List<Shift> Shifts {get;set;}=[];
        public List<Employee> Employees {get;set;}=[];
    }
    public class SpecializationDTO
    {
      public string? Name { get; set; }="";
    }
}