using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TaskDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(TaskDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
[HttpPost("login")]
[AllowAnonymous]
public IActionResult Login(LoginDto loginDto)
{
    var user = _context.Users.FirstOrDefault(u => u.UserName == loginDto.UserName);

    if (user == null)
        return Unauthorized("User not found");

    if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        return Unauthorized("Invalid password");

    var employeeExists = _context.Employees.Any(e => e.UserId == user.Id);

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        }),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"]
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Ok(new
    {
        token = tokenHandler.WriteToken(token),
        role = user.Role,
        profileCompleted = employeeExists
    });
}
    [HttpGet("users")]
    [Authorize(Roles = "Manager")]
    public IActionResult GetUsers()
    {
        var users = _context.Users
            .Select(u => new { u.Id, u.UserName, u.Role })
            .ToList();
        return Ok(users);
    }

    [HttpPut("users/{id}")]
    [Authorize(Roles = "Manager")]
    public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound("User not found");

        if (!string.IsNullOrWhiteSpace(dto.UserName))
            user.UserName = dto.UserName;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        if (!string.IsNullOrWhiteSpace(dto.Role))
            user.Role = dto.Role;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("users/{id}")]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteUser(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound("User not found");

        _context.Users.Remove(user);
        _context.SaveChanges();
        return NoContent();
    }
}

// ================= DTOs =================
public class LoginDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class UpdateUserDto
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}