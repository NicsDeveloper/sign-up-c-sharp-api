using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignUpApi.Domain.ValueObjects;

namespace SignUpApi.Infrastructure.Data.ValueConverters;

public class PasswordValueConverter : ValueConverter<Password, string>
{
    public PasswordValueConverter() : base(
        password => password.Value,
        value => Password.FromHashedPassword(value))
    {
    }
}
