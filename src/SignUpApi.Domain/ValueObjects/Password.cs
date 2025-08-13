using System.Text.RegularExpressions;

namespace SignUpApi.Domain.ValueObjects
{
  public partial class Password
  {
        public string Value { get; private set; } = string.Empty;
    
    private static readonly Regex PasswordRegex = MyRegex();

    private Password() { } // Para EF Core

    public Password(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentException("Senha não pode ser vazia", nameof(value));
      }

      if (value.Length < 8)
      {
        throw new ArgumentException("Senha deve ter pelo menos 8 caracteres", nameof(value));
      }

      if (!PasswordRegex.IsMatch(value))
      {
        throw new ArgumentException("Senha deve conter pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial", nameof(value));
      }

      Value = value;
    }

    internal Password(string hashedPassword, bool isHashed)
    {
      if (string.IsNullOrWhiteSpace(hashedPassword))
      {
        throw new ArgumentException("Senha hasheada não pode ser vazia", nameof(hashedPassword));
      }

      Value = hashedPassword;
    }

    public static Password FromHashedPassword(string hashedPassword)
    {
      return new Password(hashedPassword, true);
    }

    public static implicit operator string(Password password)
    {
      return password.Value;
    }

    public static explicit operator Password(string value)
    {
      return new(value);
    }

    public override string ToString()
    {
      return Value;
    }

    public override bool Equals(object? obj)
    {
      return obj is Password other && Value.Equals(other.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public static bool operator ==(Password left, Password right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Password left, Password right)
    {
      return !Equals(left, right);
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
  }
}
