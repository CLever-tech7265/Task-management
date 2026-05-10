using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using TaskManagement.modules;
namespace TaskManagement.Data{
    class TaskDbContext:DbContext{
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
         public DbSet<Level> EmployeeLevel { get; set; }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) 
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlServer("Server=localhost, 1434;Database=taskMangement;User Id=SA;Password=Chani,7265;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }
    }
}
