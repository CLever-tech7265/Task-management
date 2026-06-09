
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
    
     public Guid UserId { get; set; }
        public User User { get; set; } = null!;
     public string Role { get; set; } = "Employee"; // שדה חדש
    public List<EmployeeAssignedShift> AssignedEmployees { get; set; } = new();
    public List<EmployeeShiftPreference> PreferredEmployees { get; set; } = new();

    }
public class CompleteProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PeopleId { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public List<Guid> SpecializationIds { get; set; } = new();

}
    // public class EmployeeDto{
    //   public string FirstName { get; set; }="";
    //   public string LastName { get; set; }="";
    //   public string PeopleId { get; set; }="";
    //   public DateTime BirthDate { get; set; }
    //   public string Email { get; set; }="";
    // }
     public class RegisterDto
{
    public string? UserName { get; set; } = "";
    public string? Password { get; set; } = "";

    // public string? Email { get; set; } = "";
}
 
}

