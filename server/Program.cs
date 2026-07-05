using Microsoft.EntityFrameworkCore;
using TaskManagement.modules;
using TaskManagement.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDbContext")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddOpenApi();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             // Validate that the token is signed with the specified key
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),

//             // Disable issuer and audience validation for testing purposes
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//     });

var app = builder.Build();

// Configure the middleware pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagement API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Urls.Add("http://0.0.0.0:5000");
app.Run();


