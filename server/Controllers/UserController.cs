using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.modules;
using TaskManagement.Data;
using Microsoft.AspNetCore.Authorization;

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
public IActionResult Login(LoginDto loginDto)
{
    var user = _context.Users.FirstOrDefault(u => u.UserName == loginDto.UserName);
    
    if (user == null) return Unauthorized("User not found");

    // בדיקה בסיסית של סיסמה (בהמשך אפשר להוסיף hashing)
if (!BCrypt.Net.BCrypt.Verify(
        loginDto.Password,
        user.PasswordHash))
{
    return Unauthorized("Invalid password");
}
    // יצירת JWT
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new []
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
        userId = user.Id,
        role = user.Role
    });
}
}