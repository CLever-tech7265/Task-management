using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TaskManagement.modules{
    
    public class Level{
        
        public Guid  Id { get; set; }
        public string EmployeeLevel { get; set; }="";
       // public List<Employee> Employees { get; set; }
    }
    public class Employee:People{
       
      public Guid Id { get; set; }
      public string FirstName { get; set; }="";
      public string LastName { get; set; }="";
      public string PeopleId { get; set; }="";
      public DateTime BirthDate { get; set; }
      public string Email { get; set; }="";
    public List<Specialization> Spec { get; set; }
    [ForeignKey("Level")]
    public Level IdOfLevel { get; set; }
    }

}