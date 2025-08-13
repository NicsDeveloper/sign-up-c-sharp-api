using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;
using SignUpApi.Infrastructure.Data;
using SignUpApi.Infrastructure.Repositories;

namespace SignUpApi.Tests.Infrastructure;

public class UserRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task AddAsync_WithValidUser_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = new User("test@example.com", "SecurePassword123!", "João", "Silva");

        // Act
        var result = await _repository.AddAsync(user);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Email.Value.Should().Be("test@example.com");

        var savedUser = await _context.Users.FindAsync(result.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        // Arrange
        var user = new User("test@example.com", "SecurePassword123!", "João", "Silva");
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.GetByEmailAsync("test@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Value.Should().Be("test@example.com");
        result.FirstName.Should().Be("João");
        result.LastName.Should().Be("Silva");
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistingEmail_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexisting@example.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByEmailAsync_WithExistingEmail_ShouldReturnTrue()
    {
        // Arrange
        var user = new User("test@example.com", "SecurePassword123!", "João", "Silva");
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.ExistsByEmailAsync("test@example.com");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByEmailAsync_WithNonExistingEmail_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsByEmailAsync("nonexisting@example.com");

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
