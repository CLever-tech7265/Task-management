using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Specialization{
        
        public int Id { get; set; }
        public string Spec { get; set; }="";
        public List<Shift> shift {get;set;}
        public List<Employee> employees {get;set;}
    }
}