using FluentAssertions;
using Moq;
using SignUpApi.Application.Commands;
using SignUpApi.Application.Handlers;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.Interfaces;

namespace SignUpApi.Tests.Application;

public class UpdateUserProfileCommandTests
{
  private readonly Mock<IUserRepository> _mockUserRepository;
  private readonly UpdateUserProfileCommandHandler _handler;

  public UpdateUserProfileCommandTests()
  {
    _mockUserRepository = new Mock<IUserRepository>();
    _handler = new UpdateUserProfileCommandHandler(_mockUserRepository.Object);
  }

  [Fact]
  public async Task Handle_WithValidData_ShouldUpdateUserProfileSuccessfully()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserProfileCommand
    {
      UserId = userId,
      FirstName = "João Pedro",
      LastName = "Silva Santos"
    };

    var existingUser = new User("test@example.com", "SecurePassword123!", "João", "Silva");

    _ = _mockUserRepository
        .Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync(existingUser);

    _ = _mockUserRepository
        .Setup(x => x.UpdateAsync(It.IsAny<User>()))
        .ReturnsAsync(existingUser);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    _ = result.Should().NotBeNull();
    _ = result.IsSuccess.Should().BeTrue();
    _ = result.Data.Should().NotBeNull();
    _ = result.Data!.FirstName.Should().Be("João Pedro");
    _ = result.Data.LastName.Should().Be("Silva Santos");

    _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WithNonExistingUser_ShouldReturnFailureResult()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserProfileCommand
    {
      UserId = userId,
      FirstName = "João Pedro",
      LastName = "Silva Santos"
    };

    _ = _mockUserRepository
        .Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    _ = result.Should().NotBeNull();
    _ = result.IsSuccess.Should().BeFalse();
    _ = result.Errors.Should().Contain("Usuário não encontrado");

    _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
  }

  [Fact]
  public async Task Handle_WithEmptyFirstName_ShouldReturnFailureResult()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserProfileCommand
    {
      UserId = userId,
      FirstName = "",
      LastName = "Silva Santos"
    };

    var existingUser = new User("test@example.com", "SecurePassword123!", "João", "Silva");

    _ = _mockUserRepository
        .Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync(existingUser);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    _ = result.Should().NotBeNull();
    _ = result.IsSuccess.Should().BeFalse();
    _ = result.Errors.Should().Contain("Nome não pode ser vazio");

    _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
  }

  [Fact]
  public async Task Handle_WithEmptyLastName_ShouldReturnFailureResult()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserProfileCommand
    {
      UserId = userId,
      FirstName = "João Pedro",
      LastName = ""
    };

    var existingUser = new User("test@example.com", "SecurePassword123!", "João", "Silva");

    _ = _mockUserRepository
        .Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync(existingUser);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    _ = result.Should().NotBeNull();
    _ = result.IsSuccess.Should().BeFalse();
    _ = result.Errors.Should().Contain("Sobrenome não pode ser vazio");

    _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
  }
}
