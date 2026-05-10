using System.ComponentModel.DataAnnotations;
namespace TaskManagement.modules{
    public class Shift{
      
        public int Id { get; set; }
        public string StartHour { get; set; }="";
        public string FinishHour { get; set; }
        public List<Specialization> Specs { get; set; }
    }
}