using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class People{
        public string FirstName { get; set; }="";
        public string LastName { get; set; }="";
        public string PeopleId { get; set; }="";
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }="";
    }
}