using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Required for Swagger configuration
using Data;
using SQLitePCL;            // 1) Add this using
Batteries_V2.Init();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable CORS for local Angular
builder.Services.AddCors(options => {
  options.AddDefaultPolicy(policy => {
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});

// Add controllers
builder.Services.AddControllers();

// Configure Swagger services
builder.Services.AddEndpointsApiExplorer(); // Adds support for discovering API endpoints
builder.Services.AddSwaggerGen(options => {
  options.SwaggerDoc("v1", new OpenApiInfo {
    Version = "v1",
    Title = "My API",
    Description = "An API for demonstrating Swagger integration",
    Contact = new OpenApiContact {
      Name = "Your Name",
      Email = "your.email@example.com"
    }
  });
});

var app = builder.Build();

// Use CORS
app.UseCors();

// Enable Swagger middleware
if (app.Environment.IsDevelopment()) {
  app.UseSwagger(); // Generate Swagger JSON file
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = string.Empty; // Makes Swagger UI available at the root
  });
}

// Map controllers
app.MapControllers();

// Run the app
app.Run();
