using SignUpApi.Domain.ValueObjects;

namespace SignUpApi.Domain.Entities
{
  public class User
  {
    public Guid Id { get; private set; }
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { } // Para EF Core

    public User(string email, string hashedPassword, string firstName, string lastName)
    {
      Id = Guid.NewGuid();
      Email = new Email(email);
      Password = Password.FromHashedPassword(hashedPassword);

      if (string.IsNullOrWhiteSpace(firstName))
      {
        throw new ArgumentException("Nome não pode ser vazio", nameof(firstName));
      }

      FirstName = firstName.Trim();

      if (string.IsNullOrWhiteSpace(lastName))
      {
        throw new ArgumentException("Sobrenome não pode ser vazio", nameof(lastName));
      }

      LastName = lastName.Trim();

      CreatedAt = DateTime.UtcNow;
      IsActive = true;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
      if (string.IsNullOrWhiteSpace(firstName))
      {
        throw new ArgumentException("Nome não pode ser vazio", nameof(firstName));
      }

      FirstName = firstName.Trim();

      if (string.IsNullOrWhiteSpace(lastName))
      {
        throw new ArgumentException("Sobrenome não pode ser vazio", nameof(lastName));
      }

      LastName = lastName.Trim();

      UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string hashedPassword)
    {
      Password = Password.FromHashedPassword(hashedPassword);
      UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
      IsActive = false;
      UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
      IsActive = true;
      UpdatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
      LastLoginAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }

    public string GetFullName()
    {
      return $"{FirstName} {LastName}".Trim();
    }
  }
}
