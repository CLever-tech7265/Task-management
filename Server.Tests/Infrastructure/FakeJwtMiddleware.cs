using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class FakeJwtMiddleware
{
    private readonly RequestDelegate _next;

    public FakeJwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var auth = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(auth))
        {
            string role = "Employee";

            if (auth.Contains("manager"))
                role = "Manager";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "FakeJwt");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }
}