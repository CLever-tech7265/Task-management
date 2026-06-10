using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Tests
{
    public class DbContextTests
    {
        [Fact]
        public void CanAddEmployee()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb") // InMemory
    .Options;

using var context = new TaskDbContext(options);
            context.Employees.Add(new Employee { FirstName = "Hani" });
            context.SaveChanges();

            Assert.Single(context.Employees);
        }
    }
}