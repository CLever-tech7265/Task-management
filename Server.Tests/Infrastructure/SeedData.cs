using TaskManagement.Data;
using TaskManagement.modules;

public static class SeedData
{
    public static void Seed(TaskDbContext context)
    {
        context.Employees.AddRange(
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                PeopleId = "123",
                Email = "john@test.com",
                BirthDate = DateTime.UtcNow
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                PeopleId = "456",
                Email = "jane@test.com",
                BirthDate = DateTime.UtcNow
            }
        );

        context.SaveChanges();
    }
}