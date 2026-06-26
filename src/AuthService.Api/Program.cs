using AuthService.Api.Data;
using AuthService.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register context and services
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<AuthServiceManager>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
