using FluentAssertions;
using MediatR;
using Moq;
using SignUpApi.Application.Commands;
using SignUpApi.Application.DTOs;
using SignUpApi.Application.Handlers;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Tests.Application;

public class SignUpCommandTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly SignUpCommandHandler _handler;

    public SignUpCommandTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _handler = new SignUpCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object, _mockJwtTokenGenerator.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateUserAndReturnSuccess()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "SecurePassword123!",
            FirstName = "João",
            LastName = "Silva"
        };

        var hashedPassword = "hashedPassword123";
        var jwtToken = "jwt.token.here";

        _mockPasswordHasher.Setup(x => x.HashPassword(command.Password))
            .Returns(hashedPassword);

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);

        _mockJwtTokenGenerator.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(jwtToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Token.Should().Be(jwtToken);
        result.Data.User.Should().NotBeNull();
        result.Data.User.Email.Should().Be(command.Email);
        result.Data.User.FirstName.Should().Be(command.FirstName);
        result.Data.User.LastName.Should().Be(command.LastName);

        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _mockPasswordHasher.Verify(x => x.HashPassword(command.Password), Times.Once);
        _mockJwtTokenGenerator.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "existing@example.com",
            Password = "SecurePassword123!",
            FirstName = "João",
            LastName = "Silva"
        };

        var existingUser = new User(command.Email, command.Password, command.FirstName, command.LastName);

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Email já está em uso");

        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        _mockPasswordHasher.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        _mockJwtTokenGenerator.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithRepositoryError_ShouldReturnFailure()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "SecurePassword123!",
            FirstName = "João",
            LastName = "Silva"
        };

        var hashedPassword = "hashedPassword123";

        _mockPasswordHasher.Setup(x => x.HashPassword(command.Password))
            .Returns(hashedPassword);

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Erro interno do servidor");
    }
}
