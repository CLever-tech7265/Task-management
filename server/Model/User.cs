using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TaskManagement.modules

{
    public class User
    {
        
        public Guid Id { get; set; }
        public string UserName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
       //public string Email { get; set; }="";

        public string Role { get; set; } = "Employee"; // Employee / Manager

        public Employee? Employee { get; set; } // קשר 1:1
    }
    public class LoginDto
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
//     public class RegisterDto
// {
//     public string UserName { get; set; } = "";
//     public string Password { get; set; } = "";

//     public string FirstName { get; set; } = "";
//     public string LastName { get; set; } = "";
//     public string PeopleId { get; set; } = "";
//     public DateTime BirthDate { get; set; }
//     public string Email { get; set; } = "";
// }
}