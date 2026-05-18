using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Specialization{
        
        public Guid Id { get; set; }
        public string Spec { get; set; }="";
        public List<Shift> Shifts {get;set;}=[];
        public List<Employee> Employees {get;set;}=[];
    }
}