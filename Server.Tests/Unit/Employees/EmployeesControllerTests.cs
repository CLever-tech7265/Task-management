using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using TaskManagement.Controllers;
using TaskManagement.Data;
using EmployeeEntity = TaskManagement.modules.Employee;
public class EmployeesControllerTests
{
    [Fact]
    public async Task GetEmployees_ShouldReturnEmployees()
    {
        var context = TestDbContextFactory.Create();
        SeedData.Seed(context);

        var controller = new EmployeesController(context);

        var result = await controller.GetEmployees();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var employees = Assert.IsAssignableFrom<IEnumerable<EmployeeEntity>>(ok.Value);

        Assert.True(employees.Any());
    }

    [Fact]
    public async Task SearchEmployeeId_ShouldReturnOk_WhenExists()
    {
        var context = TestDbContextFactory.Create();
        SeedData.Seed(context);

        var controller = new EmployeesController(context);

        var employee = context.Employees.First();

        var result = await controller.SearchEmployeeId(employee.PeopleId);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<EmployeeEntity>(ok.Value);

        Assert.Equal(employee.PeopleId, data.PeopleId);
    }

    [Fact]
    public async Task SearchEmployeeId_ShouldReturnNotFound_WhenInvalid()
    {
        var context = TestDbContextFactory.Create();
        SeedData.Seed(context);

        var controller = new EmployeesController(context);

        var result = await controller.SearchEmployeeId("INVALID");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteEmployee_ShouldReturnOk_WhenExists()
    {
        var context = TestDbContextFactory.Create();
        SeedData.Seed(context);

        var controller = new EmployeesController(context);

        var employee = context.Employees.First();

        var result = await controller.DeleteEmployee(employee.PeopleId);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<EmployeeEntity>(ok.Value);

        Assert.Equal(employee.PeopleId, data.PeopleId);
    }

    [Fact]
    public async Task DeleteEmployee_ShouldReturnNotFound_WhenInvalid()
    {
        var context = TestDbContextFactory.Create();
        SeedData.Seed(context);

        var controller = new EmployeesController(context);

        var result = await controller.DeleteEmployee("INVALID");

        Assert.IsType<NotFoundResult>(result);
    }
}