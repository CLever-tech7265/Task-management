using Microsoft.EntityFrameworkCore;
using TaskManagement.modules;
using TaskManagement.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!
            ))
    };
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            // .AllowAnyOrigin();
            .WithOrigins( "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:5175");
    });
});

// builder.WebHost.UseUrls("http://+:8080");

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var db=scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.Migrate();

}
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

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
app.UseRouting();
app.UseCors();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

