using System.Text.RegularExpressions;

namespace SignUpApi.Domain.ValueObjects
{
  public partial class Email
  {
    public string Value { get; private set; } = string.Empty;

    private static readonly Regex EmailRegex = MyRegex();

    public Email(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentException("Email não pode ser vazio", nameof(value));
      }

      if (!EmailRegex.IsMatch(value))
      {
        throw new ArgumentException("Formato de email inválido", nameof(value));
      }

      Value = value.Trim().ToLowerInvariant();
    }

    internal Email(string emailValue, bool isFromDatabase)
    {
      Value = emailValue;
    }

    public static Email FromDatabase(string emailValue)
    {
      return new Email(emailValue, true);
    }

    public static implicit operator string(Email email)
    {
      return email.Value;
    }

    public static explicit operator Email(string value)
    {
      return new(value);
    }

    public override string ToString()
    {
      return Value;
    }

    public override bool Equals(object? obj)
    {
      return obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
      return Value.ToLowerInvariant().GetHashCode();
    }

    public static bool operator ==(Email left, Email right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Email left, Email right)
    {
      return !Equals(left, right);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "pt-BR")]
    private static partial Regex MyRegex();
  }
}
