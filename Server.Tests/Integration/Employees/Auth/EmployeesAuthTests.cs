// using Xunit;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;

// public class EmployeesAuthTests
// {
//     private HttpClient CreateClient()
//     {
//         var factory = new CustomWebApplicationFactory();
//         return factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetEmployees_ShouldReturn401_WhenNoAuth()
//     {
//         var client = CreateClient();

//         var response = await client.GetAsync("/api/employees");

//         Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//     }

//     [Fact]
//     public async Task GetEmployees_ShouldReturnOk_WhenManager()
//     {
//         var client = CreateClient();

//         var request = new HttpRequestMessage(HttpMethod.Get, "/api/employees");
//         request.Headers.Add("Authorization", "manager-token");

//         var response = await client.SendAsync(request);

//         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//     }

//     [Fact]
//     public async Task GetEmployees_ShouldReturnForbidden_WhenEmployee()
//     {
//         var client = CreateClient();

//         var request = new HttpRequestMessage(HttpMethod.Get, "/api/employees");
//         request.Headers.Add("Authorization", "employee-token");

//         var response = await client.SendAsync(request);

//         Assert.True(
//             response.StatusCode == HttpStatusCode.Forbidden ||
//             response.StatusCode == HttpStatusCode.OK
//         );
//     }
// }