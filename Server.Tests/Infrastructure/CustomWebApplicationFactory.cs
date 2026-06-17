// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using System.Linq;
// using TaskManagement.Data;
// public class CustomWebApplicationFactory : WebApplicationFactory<Program>
// {
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureServices(services =>
//         {
//             var descriptor = services.SingleOrDefault(
//                 d => d.ServiceType == typeof(DbContextOptions<TaskDbContext>));

//             if (descriptor != null)
//                 services.Remove(descriptor);

//             services.AddDbContext<TaskDbContext>(options =>
//             {
//                 options.UseInMemoryDatabase("TestDb");
//             });

//             var sp = services.BuildServiceProvider();

//             using var scope = sp.CreateScope();
//             var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
//             db.Database.EnsureCreated();
//         });

//         builder.Configure(app =>
//         {
//             app.UseMiddleware<FakeJwtMiddleware>();
//         });
//     }
// }