using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SignUpApi.Application.Commands;
using SignUpApi.Application.Handlers;
using SignUpApi.Application.Validators;
using SignUpApi.Domain.Interfaces;
using SignUpApi.Infrastructure.Data;
using SignUpApi.Infrastructure.Repositories;
using SignUpApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
if (builder.Environment.IsDevelopment())
{
    // Usar banco em memória para desenvolvimento
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("SignUpApiDb"));
}
else
{
    // Usar SQL Server para produção
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SignUpCommand).Assembly));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new JwtTokenGenerator(
        configuration["Jwt:SecretKey"] ?? "your-super-secret-key-with-at-least-32-characters",
        configuration["Jwt:Issuer"] ?? "SignUpApi",
        configuration["Jwt:Audience"] ?? "SignUpApi",
        int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60"));
});

// Validators
builder.Services.AddScoped<SignUpCommandValidator>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
