using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;
 using Microsoft.AspNetCore.Authorization;
namespace TaskManagement.Controllers{
    
[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly TaskDbContext _context;

    public AuthController(TaskDbContext context)
    {
        _context = context;
    }
[HttpPost("register")]

[Authorize(Roles = "Manager")]

public async Task<IActionResult> Register(RegisterDto dto)
{
    // בדיקה אם המשתמש כבר קיים
    if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName))
    {
        return BadRequest("User already exists");
    }

    // יצירת User
    var user = new User
    {
        Id = Guid.NewGuid(),
        UserName = dto.UserName,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = "Employee"
    };

    // יצירת Employee
    // קישור דו־כיווני
    _context.Users.Add(user);
await _context.SaveChangesAsync();
return Ok(new
    {
        message = "User registered successfully",
        user.UserName
    });
}

}
}