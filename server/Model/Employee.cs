using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TaskManagement.modules{
    
    public class Level{
        
        public Guid  Id { get; set; }
        public string EmployeeLevel { get; set; }="";
       public List<Employee> Employees { get; set; } = new();
    }
    public class Employee{
       
      public Guid Id { get; set; }
      public string FirstName { get; set; }="";
      public string LastName { get; set; }="";
      public string PeopleId { get; set; }="";
      public DateTime BirthDate { get; set; }
      public string Email { get; set; }="";
    public List<Specialization>? Specs { get; set; }=new();
    
    public Guid? LevelId { get; set; }
    public Level? Level { get; set; }
    }

    public class EmployeeDto{
      public string FirstName { get; set; }="";
      public string LastName { get; set; }="";
      public string PeopleId { get; set; }="";
      public DateTime BirthDate { get; set; }
      public string Email { get; set; }="";
    }
 
}