using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Data;
using SQLitePCL;
using Auth2FA.Services;
Batteries_V2.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source =./local.db"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TotpService>();

builder.Services.AddCors(options => {
  options.AddDefaultPolicy(policy => {
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = string.Empty;
  });
}

app.MapControllers();
app.Run();
