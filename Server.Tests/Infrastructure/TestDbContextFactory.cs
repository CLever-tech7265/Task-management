using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;

public static class TestDbContextFactory
{
    public static TaskDbContext Create()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new TaskDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}