using FluentAssertions;
using MediatR;
using Moq;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Application.Handlers;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Tests.Application;

public class LoginCommandTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _handler = new LoginCommandHandler(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenGenerator.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnSuccessResult()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "SecurePassword123!"
        };

        var user = new User("test@example.com", "SecurePassword123!", "João", "Silva");
        var token = "jwt.token.here";

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(x => x.VerifyPassword(command.Password, user.Password.Value))
            .Returns(true);

        _mockJwtTokenGenerator
            .Setup(x => x.GenerateToken(user))
            .Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().Be(token);
        result.Data.User.Email.Should().Be(command.Email);
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "nonexistent@example.com",
            Password = "SecurePassword123!"
        };

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Credenciais inválidas");
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        var user = new User("test@example.com", "SecurePassword123!", "João", "Silva");

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(x => x.VerifyPassword(command.Password, user.Password.Value))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Credenciais inválidas");
    }
}
