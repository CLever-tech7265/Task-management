using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using TaskManagement.modules;
namespace TaskManagement.Data{
    public class TaskDbContext:DbContext{
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .HasOne(u => u.Employee)
        .WithOne(e => e.User)
        .HasForeignKey<Employee>(e => e.UserId);
      // EmployeeShiftPreference

    
    // EmployeeShiftPreference
    modelBuilder.Entity<EmployeeShiftPreference>()
        .HasKey(x => x.Id);

    modelBuilder.Entity<EmployeeShiftPreference>()
        .HasOne(x => x.Employee)
        .WithMany(e => e.PreferredEmployees)
        .HasForeignKey(x => x.EmployeeId);

    modelBuilder.Entity<EmployeeShiftPreference>()
        .HasOne(x => x.Shift)
        .WithMany(s => s.PreferredEmployees)
        .HasForeignKey(x => x.ShiftId)
        .OnDelete(DeleteBehavior.Cascade);

    // EmployeeAssignedShift
    modelBuilder.Entity<EmployeeAssignedShift>()
        .HasKey(x => x.Id);

    modelBuilder.Entity<EmployeeAssignedShift>()
        .HasOne(x => x.Employee)
        .WithMany(e => e.AssignedEmployees)
        .HasForeignKey(x => x.EmployeeId);

    modelBuilder.Entity<EmployeeAssignedShift>()
        .HasOne(x => x.Shift)
        .WithMany(s => s.AssignedEmployees)
        .HasForeignKey(x => x.ShiftId)
        .OnDelete(DeleteBehavior.Cascade);
    modelBuilder.Entity<Employee>()
    .HasMany(e => e.Specs)
    .WithMany(s => s.Employees);

}
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<Level> EmployeeLevel { get; set; }
        public DbSet<modules.Task> Tasks { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<EmployeeShiftPreference> EmployeeShiftPreferences { get; set; }
public DbSet<EmployeeAssignedShift> EmployeeAssignedShifts { get; set; }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) 
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            // optionsBuilder.UseSqlServer("Server=localhost, 1434;Database=taskMangement;User Id=SA;Password=Miri,96629;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=localhost, 1434;Database=taskMangement;User Id=SA;Password=Chani,7265;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }
    }
}
