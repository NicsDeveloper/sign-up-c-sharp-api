using FluentAssertions;
using SignUpApi.Domain.Entities;
using SignUpApi.Domain.ValueObjects;

namespace SignUpApi.Tests.Domain
{
  public class UserTests
  {
    [Fact]
    public void CreateUserWithValidDataShouldCreateUserSuccessfully()
    {
      // Arrange
      string email = "test@example.com";
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!");
      string firstName = "João";
      string lastName = "Silva";

      // Act
      User user = new(email, hashedPassword, firstName, lastName);

      // Assert
      _ = user.Should().NotBeNull();
      _ = user.Email.Value.Should().Be(email);
      _ = user.Password.Value.Should().Be(hashedPassword);
      _ = user.FirstName.Should().Be(firstName);
      _ = user.LastName.Should().Be(lastName);
      _ = user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
      _ = user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void CreateUserWithInvalidEmailShouldThrowException()
    {
      // Arrange
      string invalidEmail = "invalid-email";
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!");
      string firstName = "João";
      string lastName = "Silva";

      // Act & Assert
      Func<User> action = () => new User(invalidEmail, hashedPassword, firstName, lastName);
      _ = action.Should().Throw<ArgumentException>().WithMessage("*email*");
    }

    [Fact]
    public void CreateUserWithWeakPasswordShouldThrowException()
    {
      // Arrange
      string weakPassword = "123";

      // Act & Assert
      Func<Password> action = () => new Password(weakPassword);
      _ = action.Should().Throw<ArgumentException>().WithMessage("*Senha deve ter pelo menos 8 caracteres*");
    }

    [Fact]
    public void CreateUserWithEmptyFirstNameShouldThrowException()
    {
      // Arrange
      string email = "test@example.com";
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!");
      string firstName = "";
      string lastName = "Silva";

      // Act & Assert
      Func<User> action = () => new User(email, hashedPassword, firstName, lastName);
      _ = action.Should().Throw<ArgumentException>().WithMessage("*firstName*");
    }

    [Fact]
    public void CreateUserWithEmptyLastNameShouldThrowException()
    {
      // Arrange
      string email = "test@example.com";
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!");
      string firstName = "João";
      string lastName = "";

      // Act & Assert
      Func<User> action = () => new User(email, hashedPassword, firstName, lastName);
      _ = action.Should().Throw<ArgumentException>().WithMessage("*lastName*");
    }
  }
}
