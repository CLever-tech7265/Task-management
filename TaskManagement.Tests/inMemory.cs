// using Microsoft.EntityFrameworkCore;
// using TaskManagement.Data;
// using Xunit;

// public class DbContextTests
// {
//     [Fact]
//     public void CanUseInMemoryDatabase()
//     {
//         // יוצרים options עם InMemory DB
//         var options = new DbContextOptionsBuilder<TaskDbContext>()
//             .UseInMemoryDatabase("TestDb")
//             .Options;

//         // יוצרים DbContext עם options האלו
//         using var context = new TaskDbContext(options);

//         // מוסיפים אנטרי קטן כדי לבדוק שהכל עובד
//         context.Employees.Add(new Employee { Name = "Test" });
//         context.SaveChanges();

//         // מאשרים שהאנטרי נשמר
//         Assert.Equal(1, context.Employees.Count());
//     }
// }